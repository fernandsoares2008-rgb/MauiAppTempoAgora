using MauiAppTempoAgora.Models;
using MauiAppTempoAgora.Services;

namespace MauiAppTempoAgora
{
    public partial class MainPage : ContentPage
    {
        public MainPage()
        {
            InitializeComponent();
        }

        private async void btn_buscar_Clicked(object sender, EventArgs e)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(txt_cidade.Text))
                {
                    await DisplayAlert("Atenção", "Por favor, digite o nome de uma cidade.", "OK");
                    return;
                }

                Tempo? tempo = await DataService.GetPrevisaoDoTempo(txt_cidade.Text);

                if (tempo != null)
                {
                    lbl_cidade.Text = tempo.Title;

                    // Formata a primeira letra da descrição para maiúscula de forma segura no C#
                    if (!string.IsNullOrEmpty(tempo.Description))
                    {
                        lbl_descricao.Text = char.ToUpper(tempo.Description[0]) + tempo.Description.Substring(1);
                    }
                    else
                    {
                        lbl_descricao.Text = tempo.Description;
                    }

                    lbl_temperatura.Text = $"{tempo.Temperature} ºC";
                    lbl_humidade.Text = $"Humidade: {tempo.Humidity}%";
                    lbl_vento.Text = $"Velocidade do Vento: {tempo.Wind} m/s";
                    lbl_visibilidade.Text = $"Visibilidade: {tempo.Visibility} m";

                    stk_resultado.IsVisible = true;
                }
            }
            catch (Exception ex)
            {
                if (ex.Message == "SemConexao")
                {
                    await DisplayAlert("Sem Conexão", "Você está sem acesso à internet. Verifique sua conexão e tente novamente.", "OK");
                }
                else if (ex.Message == "CidadeNaoEncontrada")
                {
                    await DisplayAlert("Cidade Não Encontrada", "A cidade informada não foi localizada. Verifique o nome digitado.", "OK");
                }
                else
                {
                    await DisplayAlert("Erro", "Ocorreu um erro ao obter os dados do clima. Tente novamente mais tarde.", "OK");
                }
            }
        }
    }
}