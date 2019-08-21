using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Box.Models
{
	public class Package
	{
		public Package()
		{
			Parts = new List<Part>();
		}
		public int ID { get; set; }
		public decimal Weight { get; set; }
		public int PartCount { get; set; }

		//Parça ve paket arasında bire-çok ilişki vardır. Bir paketin birden fazla parçası olabilir.
		public List<Part> Parts { get; set; }
		public decimal TotalCost { get; set; }
	}
}
