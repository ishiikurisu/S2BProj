using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WhatToDo.Service.Auxiliar
{
	public static class Geo
	{

		public static bool checkInsideRadius(string location, string targetLocation, double radius)
		{
			var R = 6372.8;
			double lat1, lon1;
			double lat2, lon2;

			var aux = location.Split(' ');
			lat1 = Double.Parse(aux[0]);
			lon1 = Double.Parse(aux[1]);

			aux = targetLocation.Split(' ');
			lat2 = Double.Parse(aux[0]);
			lon2 = Double.Parse(aux[1]);

			var dLat = toRadians(lat2 - lat1);
			var dLon = toRadians(lon2 - lon1);
			lat1 = toRadians(lat1);
			lat2 = toRadians(lat2);

			var a = Math.Sin(dLat / 2) * Math.Sin(dLat / 2) + Math.Sin(dLon / 2) * Math.Sin(dLon / 2) * Math.Cos(lat1) * Math.Cos(lat2);
			var c = 2 * Math.Asin(Math.Sqrt(a));
			var distance = R * 2 * Math.Asin(Math.Sqrt(a));

			return distance <= radius;

		}

		public static double toRadians(double angle)
		{
			return Math.PI * angle / 180.0;
		}
	}
}
