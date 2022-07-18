using GetMilk.ModelDTO;
using GetMilk.Repositories;
using GetMilk.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace GetMilk.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class Coletar : ContentPage
    {
        public Coletar()
        {
            InitializeComponent();
        }

        private Coleta coletaGeral = null;

        public Coletar(Coleta coleta)
        {
            InitializeComponent();

            coletaGeral = coleta;

            BindingContext = coletaGeral;
        }

        private void FecharModal(object sender, EventArgs e)
        {
            Navigation.PopModalAsync();
        }

        private async void SalvarTarefa(object sender, EventArgs e)
        {

            if (await ValidacaoAsync())
            {
                barraCarregar.IsRunning = true;

                coletaGeral.sampleNumber = numAmostra.Text;
                coletaGeral.volume = Double.Parse(volumeLitro.Text);
                coletaGeral.temperatureTank = Double.Parse(temperatura.Text);
                coletaGeral.truckCompartment = compartimento.Text;
                coletaGeral.alizarolTest = testeAlisoral.SelectedIndex == 1;

                if (await new ColetaDB().AtualizarAsync(coletaGeral))
                {
                    var current = Connectivity.NetworkAccess;

                    if (current == NetworkAccess.Internet)
                    {
                        ColetaService serColeta = new ColetaService();
                        await serColeta.IntegraColetaUnico(coletaGeral);
                    }

                    barraCarregar.IsRunning = false;

                    await DisplayAlert("Sucesso", "Coleta realizada com sucesso!", "Ok");
                    MessagingCenter.Send(new ListarColetas(), "OnColetaRealizada", coletaGeral);
                    await Navigation.PopModalAsync();
                }
            }
        }

        private async Task<bool> ValidacaoAsync()
        {
            bool valido = true;

            if (numAmostra.Text == null || numAmostra.Text == "")
            {
                await DisplayAlert("GetMilk", "O número da amostra deve ser informado!", "OK");
                valido = false;
            }

            if (volumeLitro.Text == null || volumeLitro.Text == "")
            {
                await DisplayAlert("GetMilk", "O volume deve ser informado!", "OK");
                valido = false;
            }
            else
            {
                if(Double.Parse(volumeLitro.Text) > 999)
                {
                    await DisplayAlert("GetMilk", "O volume deve ser menor que 999!", "OK");
                    valido = false;
                }
            }

            if (temperatura.Text == null || temperatura.Text == "")
            {
                await DisplayAlert("GetMilk", "A temperatura deve ser informada!", "OK");
                valido = false;
            }
            else
            {
                if (Double.Parse(temperatura.Text) > 999)
                {
                    await DisplayAlert("GetMilk", "A temperatura não pode ser maior que 50!", "OK");
                    valido = false;
                }
            }
            if (compartimento.Text == null || compartimento.Text == "")
            {
                await DisplayAlert("GetMilk", "O compartimento deve ser informado!", "OK");
                valido = false;
            }
            if (!(testeAlisoral.SelectedIndex >= 0))
            {
                await DisplayAlert("GetMilk", "O Teste de alisoral deve ser informado", "Ok");
            }

            return valido;
        }
    }
}