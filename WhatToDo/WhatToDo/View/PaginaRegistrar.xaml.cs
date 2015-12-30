using System;
using System.ComponentModel.DataAnnotations;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Popups;
using WhatToDo.Model.Entity;
using WhatToDo.Controller;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234238

namespace WhatToDo.View
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class PaginaRegistrar : Page
    {
        private MessageDialog msg;
        public PaginaRegistrar()
        {
            this.InitializeComponent();
        }

        private async void ButtonFinalizar_Click(object sender, RoutedEventArgs e)
        {
            PageRegistrarController PRC = new PageRegistrarController();
            if (!IsValidEmail(TextEmail.Text))
            {
                LabelErro.Text = "Erro! Email inválido!";
                LabelErro.Visibility = Visibility.Visible;
            }
            else if (PRC.DataBaseInsertUsuarioCaller(new Usuario(TextNome.Text, PasswordSenha.Password, TextEmail.Text)) == 0)
            {
                msg = new MessageDialog("Usuário cadastrado com sucesso!");
                await msg.ShowAsync();
                LabelErro.Visibility = Visibility.Collapsed;
                Frame.Navigate(typeof(PaginaEntrar));
            }
            else
            {
                LabelErro.Text = "Erro! Email já cadastrado!";
                LabelErro.Visibility = Visibility.Visible;
            }
        }

        bool IsValidEmail(string email)
        {
            var addr = new EmailAddressAttribute();
            return addr.IsValid(email);
        }

        private void ButtonCancelar_Click(object sender, RoutedEventArgs e)
        {
            Frame.Navigate(typeof(IntroPage));
        }
    }
}
