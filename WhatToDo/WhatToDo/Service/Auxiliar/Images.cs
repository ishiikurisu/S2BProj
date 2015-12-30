using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WhatToDo.Service.Constant;

namespace WhatToDo.Service.Auxiliar
{
    class Images
    {
        public static Uri SelectPinImage(int idCategoria)
        {
            switch (idCategoria)
            {
                case 1:
                    return Icons.Esporte;
                case 3:
                    return Icons.Festa;
                case 4:
                    return Icons.Lazer;
                case 5:
                    return Icons.Cultura;
                case 6:
                    return Icons.Curso;
                case 7:
                    return Icons.Manifestacao;
                case 8:
                    return Icons.Show;
                case 9:
                    return Icons.Culinaria;
                case 10:
                    return Icons.Comercio;
                case 11:
                    return Icons.Social;
                case 12:
                    return Icons.Religiao;
                case 13:
                    return Icons.Geocaching;
                case 14:
                    return Icons.Outros;
                default:
                    return null;
            }
        }
    }
}
