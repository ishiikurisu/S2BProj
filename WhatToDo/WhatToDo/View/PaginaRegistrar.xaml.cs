using System;
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
            //Usuario UsuarioCadastro = new Usuario(TextNome.Text, PasswordSenha.Password, TextEmail.Text);
            if (PRC.DataBaseCaller(new Usuario(TextNome.Text, PasswordSenha.Password, TextEmail.Text)) == 0)
            {
                msg = new MessageDialog("Usuário cadastrado com sucesso!");
                await msg.ShowAsync();
                LabelErro.Visibility = Visibility.Collapsed;
            }
            else
            {
                LabelErro.Visibility = Visibility.Visible;
            }
            /* turn "Collapsed" to "Visible" */
            //Old test for LabelErro.Visibility implementation.
            //if (Banco.Add(new Usuario(nome, senha, email)))
            //{
            //    var msg = new MessageDialog(string.Format("{0} uses {1} as a password. What a faggot.", nome, senha));
            //    await msg.ShowAsync();
            //}
            //else
            //{
            //    LabelErro.Visibility = Visibility.Visible;
            //}
            Frame.Navigate(typeof(MainPage), new Usuario(TextEmail.Text));
        }

        private void ButtonCancelar_Click(object sender, RoutedEventArgs e)
        {
            Frame.Navigate(typeof(IntroPage));
        }
    }
}
