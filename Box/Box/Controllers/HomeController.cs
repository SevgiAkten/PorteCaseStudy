using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Box.Models;

namespace Box.Controllers
{
	public class HomeController : Controller
	{
		public IActionResult Index()

		{
			List<Package> packages = new List<Package>
			{
				new Package {ID = 123450, Weight = 3},
				new Package {ID = 123461, Weight = 8},
				new Package {ID = 123472, Weight = 11},
				new Package {ID = 123483, Weight = 3},
				new Package {ID = 123494, Weight = 13}
			};
			// Viewe model göndermek direk data göndermekten daha sağlıklı ve daha özgür bir kullanımdır.
			var model = new PackagePartViewModel() { };

			var SortedPackages = CalculatePartCountOfEachPackage(packages);
			if (SortedPackages.Count > 0)
			{
				var PackagesWithParts = CalculatePartWeightAndCost(SortedPackages);
				foreach (var package in PackagesWithParts)
				{
					foreach (var part in package.Parts)
					{
						package.TotalCost = package.TotalCost + part.PartCost;
					}
				}
				model.Packages = PackagesWithParts;
			}

			return View(model);
		}

		public List<Package> CalculatePartCountOfEachPackage(List<Package> packages)
		{
			var SortedPackages = packages.OrderBy(x => x.Weight).ToList();
			if (SortedPackages.Count > 0)
			{
				int partCounter = 2;
				decimal PackageWeight = 0;
				foreach (var package in SortedPackages)
				{
					decimal temp = PackageWeight;
					package.PartCount = partCounter;
					for (int i = 0; i < package.PartCount; i++)
					{
						Part part = new Part();
						part.PartNumber = i + 1;
						if (temp != package.Weight)
						{
							if (package.Weight == 3)
							{
								if (i < 1)
									part.PartWeight = package.Weight % 2;
								else
									part.PartWeight = 2;

								PackageWeight = package.Weight;
							}
						}
						else
						{
							if (package.Weight % 2 != 0)
							{
								if (i < package.PartCount)
									part.PartWeight = 2;
								else
									part.PartWeight = 3;
							}
							else
							{
								part.PartWeight = 2;
							}
							PackageWeight = package.Weight;
						}
						part.PartCost = 50 + (part.PartWeight * 7);

						package.Parts.Add(part);
					}
					partCounter++;
				}
			}
			return SortedPackages;
		}
		public List<Package> CalculatePartWeightAndCost(List<Package> sortedPackages)
		{
			foreach (var package in sortedPackages)
			{
				for (int i = 0; i < package.PartCount; i++)
				{
					Part part = new Part
					{
						PartNumber = i + 1,
						PartWeight = package.Weight % package.PartCount
					};
					part.PartCost = 50 + (part.PartWeight * 7);
					package.Parts.Add(part);
				}
			}
			return sortedPackages;
		}

		public IActionResult Privacy()
		{
			return View();
		}

		[ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
		public IActionResult Error()
		{
			return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
		}
	}
}
