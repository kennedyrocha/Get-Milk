using GetMilk.ModelDTO;
using GetMilk.Repositories;
using GetMilk.Service;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace GetMilk.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class Login : ContentPage
    {
        public Login()
        {
            InitializeComponent();
        }

        public async void LogarAsync(object sender, EventArgs e)
        {
            /*]numeroCPF.Text = "12345678999";
            senha.Text = "123456";*/
            var current = Connectivity.NetworkAccess;

            if (current == NetworkAccess.Internet)
            {

                if (numeroCPF.Text == null || senha.Text == null)
                {
                    _ = DisplayAlert("GetMilk", "CPF e Senha deve ser preenchido!", "Ok");
                }
                else
                {
                    if (Regex.Replace(numeroCPF.Text, @"[^0-9]", "").Length != 11)
                    {
                        _ = DisplayAlert("GetMilk", "CPF preenchido incorretamente!", "Ok");
                        numeroCPF.Focus();
                    }
                    else
                    {
                        if (senha.Text.Length < 1)
                        {
                            _ = DisplayAlert("GetMilk", "Favor prencher a senha!", "Ok");
                            senha.Focus();
                        }
                        else
                        {
                            barraCarregar.IsRunning = true;

                            String cpf = Regex.Replace(numeroCPF.Text, @"[^0-9]", "");
                            String ValorSenha = senha.Text;

                            UsuarioService service = new UsuarioService();

                            String logar = await service.Login(cpf, ValorSenha);

                            Usuario usuario = JsonConvert.DeserializeObject<Usuario>(logar);

                            if (usuario.success == true)
                            {
                                usuario.documentCPF = cpf;
                                usuario.password = ValorSenha;

                                bool teste = App.Current.Properties.ContainsKey("UsuarioCPF");

                                if (teste == true)
                                {
                                    Application.Current.Properties["UsuarioCPF"] = cpf;
                                    Application.Current.Properties["UsuarioSenha"] = ValorSenha;
                                    Application.Current.Properties["UsuarioId"] = usuario.userId;
                                }
                                else
                                {
                                    App.Current.Properties.Add("UsuarioCPF", cpf);
                                    App.Current.Properties.Add("UsuarioSenha", ValorSenha);
                                    App.Current.Properties.Add("UsuarioId", usuario.userId);
                                }

                                teste = App.Current.Properties.ContainsKey("UsuarioCPF");

                                UsuarioDB repo = new UsuarioDB();

                                _ = await repo.CadastrarAsync(usuario);

                                VeiculoService repoVeiculo = new VeiculoService();
                                await repoVeiculo.atualizarVeiculos();

                                barraCarregar.IsRunning = false;

                                Application.Current.MainPage = new ListarServicos();
                            }
                            else
                            {
                                barraCarregar.IsRunning = false;

                                if (usuario.message == "Incorrect credentials.")
                                {
                                    _ = DisplayAlert("GetMilk", "Usuário ou senha inválida!", "Ok");
                                }
                                else
                                {
                                    _ = DisplayAlert("GetMilk", usuario.message, "Ok");
                                }
                            }
                        }
                    }
                }
            }
            else
            {
                barraCarregar.IsRunning = true;

                String cpf = Regex.Replace(numeroCPF.Text, @"[^0-9]", "");
                String ValorSenha = senha.Text;

                if (App.Current.Properties.ContainsKey("UsuarioCPF") && App.Current.Properties.ContainsKey("UsuarioId") && App.Current.Properties.ContainsKey("UsuarioSenha"))
                {
                    if (cpf == App.Current.Properties["UsuarioCPF"].ToString() && ValorSenha == App.Current.Properties["UsuarioSenha"].ToString())
                    {
                        barraCarregar.IsRunning = false;
                        Application.Current.MainPage = new ListarServicos();
                    }
                    else
                    {
                        barraCarregar.IsRunning = false;
                        await DisplayAlert("GetMilk", "Sem acesso a internet!", "Ok");
                    }
                }
                else
                {
                    barraCarregar.IsRunning = false;
                    await DisplayAlert("GetMilk", "Sem acesso a internet!", "Ok");
                }
            }
        }

        private void FormataCPF(object sender, TextChangedEventArgs e)
        {
            var ev = e as TextChangedEventArgs;

            if (ev.NewTextValue != ev.OldTextValue)
            {
                var entry = (Entry)sender;
                string text = Regex.Replace(ev.NewTextValue, @"[^0-9]", "");

                text = text.PadRight(11);

                // removendo todos os digitos excedentes 
                if (text.Length > 11)
                {
                    text = text.Remove(11);
                }

                text = text.Insert(3, ".").Insert(7, ".").Insert(11, "-").TrimEnd(new char[] { ' ', '.', '-' });
                if (entry.Text != text)
                    entry.Text = text;

            }
        }
    }
}