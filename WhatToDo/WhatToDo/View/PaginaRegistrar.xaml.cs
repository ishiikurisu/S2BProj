using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using Windows.UI.Popups;
using WhatToDo.Model.Entity;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234238

namespace WhatToDo.View
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class PaginaRegistrar : Page
    {
        public BancoDeDados Banco { get; set; }
        public PaginaRegistrar()
        {
            Banco = new BancoDeDados(new List<Usuario>());
            this.InitializeComponent();
        }

        private async void ButtonFinalizar_Click(object sender, RoutedEventArgs e)
        {
            /* turn "Collapsed" to "Visible" */
            string nome = TextNome.Text;
            string email = TextEmail.Text;
            string senha = PasswordSenha.Password;

            
            if (Banco.Add(new Usuario(nome, senha, email, "")))
            {
                var msg = new MessageDialog(string.Format("{0} uses {1} as a password. What a faggot.", nome, senha));
                await msg.ShowAsync();
            }
            else
            {
                LabelErro.Visibility = Visibility.Visible;
            }
        }
    }
}
