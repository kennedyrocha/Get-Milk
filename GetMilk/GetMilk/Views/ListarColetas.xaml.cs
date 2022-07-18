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
    public partial class ListarColetas : ContentPage
    {

        ObservableCollection<Coleta> Coletas { get; set; }

        public ListarColetas()
        {
            InitializeComponent();

            barraCarregar.IsRunning = true;

            AtualizarColetas(DateTime.Now);

            MessagingCenter.Subscribe<ListarColetas, Coleta>(this, "OnColetaRealizada", (sender, coleta) =>
            {
                if (Coletas != null)
                {
                    Coletas.Remove(coleta);
                }
            });

            barraCarregar.IsRunning = false;
        }

        private void AtualizarColetas(DateTime data)
        {
            Task.Run(() => {
                Device.BeginInvokeOnMainThread(async () => {

                    String usuario_id = App.Current.Properties["UsuarioId"].ToString();

                    Usuario usu = new UsuarioDB().Consultar(usuario_id);

                    ColetaService service = new ColetaService();
                    ColetaDB repoColeta = new ColetaDB();

                    String resposta = await service.ColetasUsuario(usu.userId);

                    UsuarioColeta coletas = JsonConvert.DeserializeObject<UsuarioColeta>(resposta);

                    if (coletas.success == true)
                    {
                        foreach (var item in coletas.collects.ToList<Coleta>())
                        {
                            _ = await repoColeta.CadastrarAsync(item);
                        }
                    }

                    Coletas = new ObservableCollection<Coleta>(
                        await repoColeta.PesquisarAsync()
                    );

                    CVListaDeColeta.ItemsSource = Coletas;
                    numColetas.Text = Coletas.Count.ToString();
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

        private async void BtnColetaAsync(object sender, EventArgs e)
        {
            var evento = (Button)sender;
            var coleta = (Coleta)evento.BindingContext;

            await Navigation.PushModalAsync(new Coletar(coleta));
        }

        private void BtnIrParaColetas(object sender, EventArgs e)
        {
            Navigation.PushModalAsync(new ListarColetas());
        }
    }
}