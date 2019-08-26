using System;
using System.Linq;
using Constructor.Database;
using Starcounter.Nova;

namespace Constructor
{
    public class TestData
    {
        public void CreateDefaultBicycleProduct()
        {
            Db.Transact(() =>
            {
                Repository repository = new Repository("Bicycle Repository");
                Branch branch = repository.Branches.First(x => x.Parent == null);

                branch.StartEdit();
                repository.CurrentCommit.Name = "Create Bicycle";

                Product product = new Product(repository)
                {
                    Name = "Bicycle",
                    ImageUrl = "/Constructor/images/bicycle.jpg"
                };

                branch.FinishEdit();
                branch.StartEdit();
                repository.CurrentCommit.Name = "Fill in modules";

                new Module(product)
                {
                    Name = "Frame",
                    Quantity = 1,
                    Price = 2199,
                    SortIndex = 1,
                    Description = "Brand-X RD-01 Racerram",
                    ImageUrl = "/Constructor/images/bicycle-frame.jpg"
                };

                new Module(product)
                {
                    Name = "Wheel",
                    Quantity = 2,
                    Price = 124,
                    SortIndex = 2,
                    Description = "Hjulset Shimano Wh-r501 Kanttråd Shimano",
                    ImageUrl = "/Constructor/images/bicycle-wheel.jpg"
                };

                new Module(product)
                {
                    Name = "Chain",
                    Quantity = 1,
                    Price = 25,
                    SortIndex = 3,
                    Description = "Shimano 105 5800 11-speed Sil-tec",
                    ImageUrl = "/Constructor/images/bicycle-chain.jpg"
                };

                new Module(product)
                {
                    Name = "Gearing",
                    Quantity = 1,
                    Price = 21,
                    SortIndex = 4,
                    Description = "Kassett Shimano Cs-hg400 9 Växlar 11-32t",
                    ImageUrl = "/Constructor/images/bicycle-cogs.jpg"
                };

                new Module(product)
                {
                    Name = "Steering bar",
                    Quantity = 1,
                    Price = 17,
                    SortIndex = 5,
                    Description = "Xlc Riser-bar Hb-m04",
                    ImageUrl = "/Constructor/images/bicycle-steering-bar.jpg"
                };

                new Module(product)
                {
                    Name = "Seat",
                    Quantity = 1,
                    Price = 87,
                    SortIndex = 6,
                    Description = "Fizik Arione Sadel 130mm",
                    ImageUrl = "/Constructor/images/bicycle-seat.jpg"
                };

                new Module(product)
                {
                    Name = "Brakes",
                    Quantity = 2,
                    Price = 58,
                    SortIndex = 7,
                    Description = "Shimano Deore M6000 Skivbromsok",
                    ImageUrl = "/Constructor/images/bicycle-brakes.jpg"
                };

                branch.FinishEdit();
            });
        }

        public void CreateDefaultComputerProduct()
        {
            Db.Transact(() =>
            {
                Random rand = new Random();
                Repository repository = new Repository("Computer Repository");
                Branch branch = repository.Branches.First(x => x.Parent == null);
                var productImages = new[]
                {
                    "/Constructor/images/computer-0.jpg",
                    "/Constructor/images/computer-1.png",
                    "/Constructor/images/computer-2.png",
                    "/Constructor/images/computer-3.jpg",
                    "/Constructor/images/computer-4.jpg",
                    "/Constructor/images/computer-5.jpg",
                    "/Constructor/images/computer-6.jpg",
                    "/Constructor/images/computer-7.jpg",
                    "/Constructor/images/computer-8.jpg",
                    "/Constructor/images/computer-9.jpg"
                };

                branch.StartEdit();
                repository.CurrentCommit.Name = "Assemble Computer";

                Product product = new Product(repository)
                {
                    Name = "Gaming Computer",
                    ImageUrl = productImages[rand.Next(0, productImages.Length)]
                };

                new Module(product)
                {
                    Name = "CPU",
                    Description = "AMD Ryzen 5 2600",
                    Price = 148,
                    Quantity = 1,
                    SortIndex = 1,
                    ImageUrl = "/Constructor/images/amd-ryzen-5-2600.jpg"
                };

                new Module(product)
                {
                    Name = "CPU Cooler",
                    Description = "Cooler Master Hyper 212 EVO",
                    Price = 24,
                    Quantity = 1,
                    SortIndex = 2,
                    ImageUrl = "/Constructor/images/cooler-master-hyper-212-evo.jpg"
                };

                new Module(product)
                {
                    Name = "Motherboard",
                    Description = "MSI B450 TOMAHAWK",
                    Price = 99,
                    Quantity = 1,
                    SortIndex = 3,
                    ImageUrl = "/Constructor/images/msi-b450-tomahawk.jpg"
                };

                new Module(product)
                {
                    Name = "RAM",
                    Description = "Corsair Vengeance LPX 8 GB",
                    Price = 39,
                    Quantity = 2,
                    SortIndex = 4,
                    ImageUrl = "/Constructor/images/corsair-vengeance-lpx-8-gb.jpg"
                };

                new Module(product)
                {
                    Name = "SSD",
                    Description = "Samsung 860 Evo 500 GB",
                    Price = 77,
                    Quantity = 1,
                    SortIndex = 5,
                    ImageUrl = "/Constructor/images/samsung-860-evo-500-gb.jpg"
                };

                new Module(product)
                {
                    Name = "GPU",
                    Description = "MSI Radeon RX 570 8 GB ARMOR OC",
                    Price = 154,
                    Quantity = 1,
                    SortIndex = 6,
                    ImageUrl = "/Constructor/images/msi-radeon-rx-570-8-bg-armor-oc.jpg"
                };

                new Module(product)
                {
                    Name = "Case",
                    Description = "Fractal Design Focus G",
                    Price = 58,
                    Quantity = 1,
                    SortIndex = 7,
                    ImageUrl = "/Constructor/images/fractal-design-focus-g.jpg"
                };

                new Module(product)
                {
                    Name = "PSU",
                    Description = "EVGA SuperNOVA G3 750 W",
                    Price = 109,
                    Quantity = 1,
                    SortIndex = 8,
                    ImageUrl = "/Constructor/images/evga-supernova-g3-750w.jpg"
                };

                branch.FinishEdit();
                branch.StartEdit();
                repository.CurrentCommit.Name = "Install Windows";

                new Module(product)
                {
                    Name = "OS",
                    Description = "Microsoft Windows 10 Pro",
                    Price = 139,
                    Quantity = 1,
                    SortIndex = 9,
                    ImageUrl = "/Constructor/images/microsoft-windows-10-pro.jpg"
                };

                branch.FinishEdit();
                branch.StartEdit();
                repository.CurrentCommit.Name = "Upgrade GPU";

                product.Modules.First(x => x.Name == "GPU").IsDeleted = true;

                new Module(product)
                {
                    Name = "GPU",
                    Description = "MSI GeForce RTX 2070 8 GB GAMING",
                    Price = 519,
                    Quantity = 1,
                    SortIndex = 6,
                    ImageUrl = "/Constructor/images/msi-geforce-rtx-2070-8-gb-gaming.jpg"
                };

                branch.FinishEdit();

                branch = new Branch("AMD on 7nm", branch);
                branch.StartEdit();
                repository.CurrentBranch = branch;
                repository.CurrentCommit = branch.GetLastOwnCommit();

                repository.CurrentCommit.Name = "Upgrade to 3d generation Ryzen";
                product.Modules.First(x => x.Name == "CPU").IsDeleted = true;

                new Module(product)
                {
                    Name = "CPU",
                    Description = "AMD Ryzen 5 3600",
                    Price = 199,
                    Quantity = 1,
                    SortIndex = 1,
                    ImageUrl = "/Constructor/images/amd-ryzen-5-3600.jpg"
                };

                branch.FinishEdit();
                branch.StartEdit();
                repository.CurrentCommit.Name = "Upgrade to Navi GPU";
                product.Modules.First(x => x.Name == "GPU").IsDeleted = true;

                new Module(product)
                {
                    Name = "GPU",
                    Description = "XFX Radeon RX 5700 8 GB",
                    Price = 349,
                    Quantity = 1,
                    SortIndex = 6,
                    ImageUrl = "/Constructor/images/xfx-radeon-rx-5700.jpg"
                };

                branch.FinishEdit();
                branch = repository.Branches.First(x => x.Parent == null);
                repository.CurrentBranch = branch;
                repository.CurrentCommit = branch.GetLastOwnCommit();
            });
        }
    }
}
