using GetMilk.ModelDTO;
using GetMilk.Repositories;
using GetMilk.Service;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace GetMilk.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ListarServicos : ContentPage
    {
        ObservableCollection<Servico> Servicos { get; set; }

        public ListarServicos()
        {
            InitializeComponent();

            AtualizarServicos(DateTime.Now);
        }

        private void DateSelectedAction(object sender, DateChangedEventArgs e)
        {
            //e.NewDate
            AtualizarServicos(e.NewDate);
        }

        private void AtualizarServicos(DateTime data)
        {
            Task.Run(() => {
                Device.BeginInvokeOnMainThread(async () => {

                    String usuario_id = App.Current.Properties["UsuarioId"].ToString();

                    Usuario usu = new UsuarioDB().Consultar(usuario_id);

                    BemVindo.Text = usu.userName;

                    ServicoService service = new ServicoService();
                    ServicoDB repoServico = new ServicoDB();

                    String resposta = await service.ServicosUsuario(usu.userId);

                    UsuarioServico servicos = JsonConvert.DeserializeObject<UsuarioServico>(resposta);
                    List<string> placas = new List<string>();

                    if (servicos.success == true)
                    {
                        foreach (var item in servicos.services.ToList<Servico>())
                        {
                            placas.Add(item.licensePlate);
                            _ = await repoServico.CadastrarAsync(item);
                        }
                    }

                    VeiculoService usuVeiculo = new VeiculoService();
                    await usuVeiculo.verificaVeiculos(placas);

                    Servicos = new ObservableCollection<Servico>(
                        await repoServico.PesquisarAsync()
                    );
                    CVListaDeServicos.ItemsSource = Servicos;

                    foreach (var item in Servicos)
                    {
                        if (item.NomeBotao == "Finalizar")
                        {
                            await Navigation.PushModalAsync(new ListarColetas());
                            break;
                        }
                    }

                    carregando.IsRunning = false;
                });
            });

            var idioma = CultureInfo.CurrentCulture;

            Dia.Text = data.Day.ToString();
            Mes.Text = PrimeiraLetraMaiuscula(data.ToString("MMMM", idioma)).Substring(0, 3);

            DiaSemana.Text = PrimeiraLetraMaiuscula(idioma.DateTimeFormat.GetDayName(data.DayOfWeek));
        }

        private string PrimeiraLetraMaiuscula(string palavra)
        {
            return char.ToUpper(palavra[0]) + palavra.Substring(1);
        }

        private async void BtnServicoAsync(object sender, EventArgs e)
        {
            var evento = (Button)sender;
            var servico = (Servico)evento.BindingContext;

            if (evento.Text == "Iniciar")
            {
                string result = await DisplayPromptAsync("Valor Odômetro", "Favor informar o valor inical do odômetro?", keyboard: Keyboard.Numeric);

                if (result != null && result == "")
                {
                    await DisplayAlert("GetMilk", "O odômetro inicial deve ser informado", "Ok", "Cancelar");
                }
                else if (result != null)
                {
                    ServicoDB repo = new ServicoDB();
                    ServicoService integrador = new ServicoService();

                    servico.odometerStart = Int32.Parse(result);

                    carregando.IsRunning = true;

                    await repo.UpdateAsync(servico);
                    await integrador.IntegraServicoUnico(servico);

                    carregando.IsRunning = false;

                    evento.Text = "Finalizar";
                    await Navigation.PushModalAsync(new ListarColetas());
                }
            }

            else if (evento.Text == "Finalizar")
            {
                string result = await DisplayPromptAsync("Valor Odômetro", "Favor informar o valor final do odômetro?", keyboard: Keyboard.Numeric);

                if (result != null && result == "")
                {
                    await DisplayAlert("GetMilk", "O odômetro final deve ser informado", "Ok");
                }
                else if (result != null)
                {
                    int valorFinal = Int32.Parse(result);

                    if (valorFinal <= servico.odometerStart)
                    {
                        await DisplayAlert("GetMilk", "O odômetro final deve ser maior que o inicial", "Ok");
                    }
                    else
                    {
                        ServicoDB repo = new ServicoDB();
                        ServicoService integrador = new ServicoService();

                        servico.odometerEnd = valorFinal;

                        carregando.IsRunning = true;

                        _ = repo.UpdateAsync(servico);
                        await integrador.IntegraServicoUnico(servico);

                        carregando.IsRunning = false;

                        evento.Text = "Finalizado";
                        evento.IsEnabled = false;

                        App.Current.MainPage = new Views.Login();
                        Application.Current.Properties["UsuarioCPF"] = "a";
                        Application.Current.Properties["UsuarioSenha"] = "a";
                        Application.Current.Properties["UsuarioId"] = "a";

                    }
                }
            }
        }

        private void BtnIrParaColetas(object sender, EventArgs e)
        {
            Navigation.PushModalAsync(new ListarColetas());
        }

        private async void SairDoAplicativo(object sender, EventArgs e)
        {
            string resposta = await DisplayActionSheet("Deseja realmente sair do GetMilk?", "Cancelar", "Sim");

            if (resposta == "Sim")
            {
                App.Current.MainPage = new Views.Login();
            }
        }
    }
}