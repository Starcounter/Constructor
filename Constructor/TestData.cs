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
                Repository repository = Repository.Create("Bicycle Repository");
                Branch branch = repository.Branches.First(x => x.ParentBranch == null);

                branch.StartEdit();
                repository.CurrentCommit.Name = "Create Bicycle";

                Product product = Product.Create(repository);
                product.Name = "Bicycle";
                product.ImageUrl = "/Constructor/images/bicycle.jpg";

                branch.FinishEdit();
                branch.StartEdit();
                repository.CurrentCommit.Name = "Fill in modules";

                var frame = Module.Create(product);
                frame.Name = "Frame";
                frame.Quantity = 1;
                frame.Price = 2199;
                frame.SortIndex = 1;
                frame.Description = "Brand-X RD-01 Racerram";
                frame.ImageUrl = "/Constructor/images/bicycle-frame.jpg";

                var wheel = Module.Create(product);
                wheel.Name = "Wheel";
                wheel.Quantity = 2;
                wheel.Price = 124;
                wheel.SortIndex = 2;
                wheel.Description = "Hjulset Shimano Wh-r501 Kanttråd Shimano";
                wheel.ImageUrl = "/Constructor/images/bicycle-wheel.jpg";

                var chain = Module.Create(product);
                chain.Name = "Chain";
                chain.Quantity = 1;
                chain.Price = 25;
                chain.SortIndex = 3;
                chain.Description = "Shimano 105 5800 11-speed Sil-tec";
                chain.ImageUrl = "/Constructor/images/bicycle-chain.jpg";

                var gearing = Module.Create(product);
                gearing.Name = "Gearing";
                gearing.Quantity = 1;
                gearing.Price = 21;
                gearing.SortIndex = 4;
                gearing.Description = "Kassett Shimano Cs-hg400 9 Växlar 11-32t";
                gearing.ImageUrl = "/Constructor/images/bicycle-cogs.jpg";


                var steeringBar = Module.Create(product);
                steeringBar.Name = "Steering bar";
                steeringBar.Quantity = 1;
                steeringBar.Price = 17;
                steeringBar.SortIndex = 5;
                steeringBar.Description = "Xlc Riser-bar Hb-m04";
                steeringBar.ImageUrl = "/Constructor/images/bicycle-steering-bar.jpg";

                var seat = Module.Create(product);
                seat.Name = "Seat";
                seat.Quantity = 1;
                seat.Price = 87;
                seat.SortIndex = 6;
                seat.Description = "Fizik Arione Sadel 130mm";
                seat.ImageUrl = "/Constructor/images/bicycle-seat.jpg";

                var brakes = Module.Create(product);
                brakes.Name = "Brake";
                brakes.Quantity = 2;
                brakes.Price = 58;
                brakes.SortIndex = 7;
                brakes.Description = "Shimano Deore M6000 Skivbromsok";
                brakes.ImageUrl = "/Constructor/images/bicycle-brakes.jpg";

                branch.FinishEdit();
            });
        }

        public void CreateDefaultComputerProduct()
        {
            Db.Transact(() =>
            {
                Random rand = new Random();
                Repository repository = Repository.Create("Computer Repository");
                Branch branch = repository.Branches.First(x => x.ParentBranch == null);
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

                Product product = Product.Create(repository);
                product.Name = "Gaming Computer";
                product.ImageUrl = productImages[rand.Next(0, productImages.Length)];

                var cpu = Module.Create(product);
                cpu.Name = "CPU";
                cpu.Description = "AMD Ryzen 5 2600";
                cpu.Price = 148;
                cpu.Quantity = 1;
                cpu.SortIndex = 1;
                cpu.ImageUrl = "/Constructor/images/amd-ryzen-5-2600.jpg";

                var cpuCooler = Module.Create(product);
                cpuCooler.Name = "CPU Cooler";
                cpuCooler.Description = "Cooler Master Hyper 212 EVO";
                cpuCooler.Price = 24;
                cpuCooler.Quantity = 1;
                cpuCooler.SortIndex = 2;
                cpuCooler.ImageUrl = "/Constructor/images/cooler-master-hyper-212-evo.jpg";

                var motherboard = Module.Create(product);
                motherboard.Name = "Motherboard";
                motherboard.Description = "MSI B450 TOMAHAWK";
                motherboard.Price = 99;
                motherboard.Quantity = 1;
                motherboard.SortIndex = 3;
                motherboard.ImageUrl = "/Constructor/images/msi-b450-tomahawk.jpg";

                var ram = Module.Create(product);
                ram.Name = "RAM";
                ram.Description = "Corsair Vengeance LPX 8 GB";
                ram.Price = 39;
                ram.Quantity = 2;
                ram.SortIndex = 4;
                ram.ImageUrl = "/Constructor/images/corsair-vengeance-lpx-8-gb.jpg";

                var ssd = Module.Create(product);
                ssd.Name = "SSD";
                ssd.Description = "Samsung 860 Evo 500 GB";
                ssd.Price = 77;
                ssd.Quantity = 1;
                ssd.SortIndex = 5;
                ssd.ImageUrl = "/Constructor/images/samsung-860-evo-500-gb.jpg";

                var radeon1 = Module.Create(product);
                radeon1.Name = "GPU";
                radeon1.Description = "MSI Radeon RX 570 8 GB ARMOR OC";
                radeon1.Price = 154;
                radeon1.Quantity = 1;
                radeon1.SortIndex = 6;
                radeon1.ImageUrl = "/Constructor/images/msi-radeon-rx-570-8-bg-armor-oc.jpg";

                var @case = Module.Create(product);
                @case.Name = "Case";
                @case.Description = "Fractal Design Focus G";
                @case.Price = 58;
                @case.Quantity = 1;
                @case.SortIndex = 7;
                @case.ImageUrl = "/Constructor/images/fractal-design-focus-g.jpg";

                var psu = Module.Create(product);
                psu.Name = "PSU";
                psu.Description = "EVGA SuperNOVA G3 750 W";
                psu.Price = 109;
                psu.Quantity = 1;
                psu.SortIndex = 8;
                psu.ImageUrl = "/Constructor/images/evga-supernova-g3-750w.jpg";

                branch.FinishEdit();
                branch.StartEdit();
                repository.CurrentCommit.Name = "Install Windows";

                var os = Module.Create(product);
                os.Name = "OS";
                os.Description = "Microsoft Windows 10 Pro";
                os.Price = 139;
                os.Quantity = 1;
                os.SortIndex = 9;
                os.ImageUrl = "/Constructor/images/microsoft-windows-10-pro.jpg";

                branch.FinishEdit();
                branch.StartEdit();
                repository.CurrentCommit.Name = "Upgrade GPU";

                product.Modules.First(x => x.Name == "GPU").IsDeleted = true;

                var geforce = Module.Create(product);
                geforce.Name = "GPU";
                geforce.Description = "MSI GeForce RTX 2070 8 GB GAMING";
                geforce.Price = 519;
                geforce.Quantity = 1;
                geforce.SortIndex = 6;
                geforce.ImageUrl = "/Constructor/images/msi-geforce-rtx-2070-8-gb-gaming.jpg";

                branch.FinishEdit();

                branch = Branch.Create("AMD on 7nm", branch);
                branch.StartEdit();
                repository.CurrentBranch = branch;
                repository.CurrentCommit = branch.GetLastOwnCommit();

                repository.CurrentCommit.Name = "Upgrade to 3d generation Ryzen";
                product.Modules.First(x => x.Name == "CPU").IsDeleted = true;

                var ryzen5 = Module.Create(product);
                ryzen5.Name = "CPU";
                ryzen5.Description = "AMD Ryzen 5 3600";
                ryzen5.Price = 199;
                ryzen5.Quantity = 1;
                ryzen5.SortIndex = 1;
                ryzen5.ImageUrl = "/Constructor/images/amd-ryzen-5-3600.jpg";

                branch.FinishEdit();
                branch.StartEdit();
                repository.CurrentCommit.Name = "Upgrade to Navi GPU";
                product.Modules.First(x => x.Name == "GPU").IsDeleted = true;

                var radeon2 = Module.Create(product);
                radeon2.Name = "GPU";
                radeon2.Description = "XFX Radeon RX 5700 8 GB";
                radeon2.Price = 349;
                radeon2.Quantity = 1;
                radeon2.SortIndex = 6;
                radeon2.ImageUrl = "/Constructor/images/xfx-radeon-rx-5700.jpg";

                branch.FinishEdit();
                branch = repository.Branches.First(x => x.ParentBranch == null);
                repository.CurrentBranch = branch;
                repository.CurrentCommit = branch.GetLastOwnCommit();
            });
        }
    }
}