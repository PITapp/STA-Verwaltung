using System;
using System.Data;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Text.Encodings.Web;
using Microsoft.AspNetCore.Components;
using Microsoft.EntityFrameworkCore;
using Radzen;

using STAVerwaltung.Data;

namespace STAVerwaltung
{
    public partial class dbSTAVerwaltungService
    {
        dbSTAVerwaltungContext Context
        {
           get
           {
             return this.context;
           }
        }

        private readonly dbSTAVerwaltungContext context;
        private readonly NavigationManager navigationManager;

        public dbSTAVerwaltungService(dbSTAVerwaltungContext context, NavigationManager navigationManager)
        {
            this.context = context;
            this.navigationManager = navigationManager;
        }

        public void Reset() => Context.ChangeTracker.Entries().Where(e => e.Entity != null).ToList().ForEach(e => e.State = EntityState.Detached);

        public void ApplyQuery<T>(ref IQueryable<T> items, Query query = null)
        {
            if (query != null)
            {
                if (!string.IsNullOrEmpty(query.Filter))
                {
                    if (query.FilterParameters != null)
                    {
                        items = items.Where(query.Filter, query.FilterParameters);
                    }
                    else
                    {
                        items = items.Where(query.Filter);
                    }
                }

                if (!string.IsNullOrEmpty(query.OrderBy))
                {
                    items = items.OrderBy(query.OrderBy);
                }

                if (query.Skip.HasValue)
                {
                    items = items.Skip(query.Skip.Value);
                }

                if (query.Top.HasValue)
                {
                    items = items.Take(query.Top.Value);
                }
            }
        }


        public async Task ExportAdressenToExcel(Query query = null, string fileName = null)
        {
            navigationManager.NavigateTo(query != null ? query.ToUrl($"export/dbstaverwaltung/adressen/excel(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')") : $"export/dbstaverwaltung/adressen/excel(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')", true);
        }

        public async Task ExportAdressenToCSV(Query query = null, string fileName = null)
        {
            navigationManager.NavigateTo(query != null ? query.ToUrl($"export/dbstaverwaltung/adressen/csv(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')") : $"export/dbstaverwaltung/adressen/csv(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')", true);
        }

        partial void OnAdressenRead(ref IQueryable<STAVerwaltung.Models.dbSTAVerwaltung.Adressen> items);

        public async Task<IQueryable<STAVerwaltung.Models.dbSTAVerwaltung.Adressen>> GetAdressen(Query query = null)
        {
            var items = Context.Adressen.AsQueryable();

            items = items.Include(i => i.AdressenArten);
            items = items.Include(i => i.AdressenAnreden);
            items = items.Include(i => i.Bundeslaender);
            items = items.Include(i => i.AdressenFamilienstaende);
            items = items.Include(i => i.Gemeinden);
            items = items.Include(i => i.AdressenGeschlechter);
            items = items.Include(i => i.LKZ1);
            items = items.Include(i => i.AdressenSortierungFamilie);

            if (query != null)
            {
                if (!string.IsNullOrEmpty(query.Expand))
                {
                    var propertiesToExpand = query.Expand.Split(',');
                    foreach(var p in propertiesToExpand)
                    {
                        items = items.Include(p.Trim());
                    }
                }

                ApplyQuery(ref items, query);
            }

            OnAdressenRead(ref items);

            return await Task.FromResult(items);
        }

        partial void OnAdressenGet(STAVerwaltung.Models.dbSTAVerwaltung.Adressen item);
        partial void OnGetAdressenByAdressNr(ref IQueryable<STAVerwaltung.Models.dbSTAVerwaltung.Adressen> items);


        public async Task<STAVerwaltung.Models.dbSTAVerwaltung.Adressen> GetAdressenByAdressNr(int adressnr)
        {
            var items = Context.Adressen
                              .AsNoTracking()
                              .Where(i => i.AdressNr == adressnr);

            items = items.Include(i => i.AdressenArten);
            items = items.Include(i => i.AdressenAnreden);
            items = items.Include(i => i.Bundeslaender);
            items = items.Include(i => i.AdressenFamilienstaende);
            items = items.Include(i => i.Gemeinden);
            items = items.Include(i => i.AdressenGeschlechter);
            items = items.Include(i => i.LKZ1);
            items = items.Include(i => i.AdressenSortierungFamilie);
 
            OnGetAdressenByAdressNr(ref items);

            var itemToReturn = items.FirstOrDefault();

            OnAdressenGet(itemToReturn);

            return await Task.FromResult(itemToReturn);
        }

        partial void OnAdressenCreated(STAVerwaltung.Models.dbSTAVerwaltung.Adressen item);
        partial void OnAfterAdressenCreated(STAVerwaltung.Models.dbSTAVerwaltung.Adressen item);

        public async Task<STAVerwaltung.Models.dbSTAVerwaltung.Adressen> CreateAdressen(STAVerwaltung.Models.dbSTAVerwaltung.Adressen adressen)
        {
            OnAdressenCreated(adressen);

            var existingItem = Context.Adressen
                              .Where(i => i.AdressNr == adressen.AdressNr)
                              .FirstOrDefault();

            if (existingItem != null)
            {
               throw new Exception("Item already available");
            }            

            try
            {
                Context.Adressen.Add(adressen);
                Context.SaveChanges();
            }
            catch
            {
                Context.Entry(adressen).State = EntityState.Detached;
                throw;
            }

            OnAfterAdressenCreated(adressen);

            return adressen;
        }

        public async Task<STAVerwaltung.Models.dbSTAVerwaltung.Adressen> CancelAdressenChanges(STAVerwaltung.Models.dbSTAVerwaltung.Adressen item)
        {
            var entityToCancel = Context.Entry(item);
            if (entityToCancel.State == EntityState.Modified)
            {
              entityToCancel.CurrentValues.SetValues(entityToCancel.OriginalValues);
              entityToCancel.State = EntityState.Unchanged;
            }

            return item;
        }

        partial void OnAdressenUpdated(STAVerwaltung.Models.dbSTAVerwaltung.Adressen item);
        partial void OnAfterAdressenUpdated(STAVerwaltung.Models.dbSTAVerwaltung.Adressen item);

        public async Task<STAVerwaltung.Models.dbSTAVerwaltung.Adressen> UpdateAdressen(int adressnr, STAVerwaltung.Models.dbSTAVerwaltung.Adressen adressen)
        {
            OnAdressenUpdated(adressen);

            var itemToUpdate = Context.Adressen
                              .Where(i => i.AdressNr == adressen.AdressNr)
                              .FirstOrDefault();

            if (itemToUpdate == null)
            {
               throw new Exception("Item no longer available");
            }
                
            var entryToUpdate = Context.Entry(itemToUpdate);
            entryToUpdate.CurrentValues.SetValues(adressen);
            entryToUpdate.State = EntityState.Modified;

            Context.SaveChanges();

            OnAfterAdressenUpdated(adressen);

            return adressen;
        }

        partial void OnAdressenDeleted(STAVerwaltung.Models.dbSTAVerwaltung.Adressen item);
        partial void OnAfterAdressenDeleted(STAVerwaltung.Models.dbSTAVerwaltung.Adressen item);

        public async Task<STAVerwaltung.Models.dbSTAVerwaltung.Adressen> DeleteAdressen(int adressnr)
        {
            var itemToDelete = Context.Adressen
                              .Where(i => i.AdressNr == adressnr)
                              .Include(i => i.AdressenEreignisse)
                              .FirstOrDefault();

            if (itemToDelete == null)
            {
               throw new Exception("Item no longer available");
            }

            OnAdressenDeleted(itemToDelete);


            Context.Adressen.Remove(itemToDelete);

            try
            {
                Context.SaveChanges();
            }
            catch
            {
                Context.Entry(itemToDelete).State = EntityState.Unchanged;
                throw;
            }

            OnAfterAdressenDeleted(itemToDelete);

            return itemToDelete;
        }
    
        public async Task ExportAdressenAnredenToExcel(Query query = null, string fileName = null)
        {
            navigationManager.NavigateTo(query != null ? query.ToUrl($"export/dbstaverwaltung/adressenanreden/excel(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')") : $"export/dbstaverwaltung/adressenanreden/excel(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')", true);
        }

        public async Task ExportAdressenAnredenToCSV(Query query = null, string fileName = null)
        {
            navigationManager.NavigateTo(query != null ? query.ToUrl($"export/dbstaverwaltung/adressenanreden/csv(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')") : $"export/dbstaverwaltung/adressenanreden/csv(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')", true);
        }

        partial void OnAdressenAnredenRead(ref IQueryable<STAVerwaltung.Models.dbSTAVerwaltung.AdressenAnreden> items);

        public async Task<IQueryable<STAVerwaltung.Models.dbSTAVerwaltung.AdressenAnreden>> GetAdressenAnreden(Query query = null)
        {
            var items = Context.AdressenAnreden.AsQueryable();


            if (query != null)
            {
                if (!string.IsNullOrEmpty(query.Expand))
                {
                    var propertiesToExpand = query.Expand.Split(',');
                    foreach(var p in propertiesToExpand)
                    {
                        items = items.Include(p.Trim());
                    }
                }

                ApplyQuery(ref items, query);
            }

            OnAdressenAnredenRead(ref items);

            return await Task.FromResult(items);
        }

        partial void OnAdressenAnredenGet(STAVerwaltung.Models.dbSTAVerwaltung.AdressenAnreden item);
        partial void OnGetAdressenAnredenByAnredeCode(ref IQueryable<STAVerwaltung.Models.dbSTAVerwaltung.AdressenAnreden> items);


        public async Task<STAVerwaltung.Models.dbSTAVerwaltung.AdressenAnreden> GetAdressenAnredenByAnredeCode(string anredecode)
        {
            var items = Context.AdressenAnreden
                              .AsNoTracking()
                              .Where(i => i.AnredeCode == anredecode);

 
            OnGetAdressenAnredenByAnredeCode(ref items);

            var itemToReturn = items.FirstOrDefault();

            OnAdressenAnredenGet(itemToReturn);

            return await Task.FromResult(itemToReturn);
        }

        partial void OnAdressenAnredenCreated(STAVerwaltung.Models.dbSTAVerwaltung.AdressenAnreden item);
        partial void OnAfterAdressenAnredenCreated(STAVerwaltung.Models.dbSTAVerwaltung.AdressenAnreden item);

        public async Task<STAVerwaltung.Models.dbSTAVerwaltung.AdressenAnreden> CreateAdressenAnreden(STAVerwaltung.Models.dbSTAVerwaltung.AdressenAnreden adressenanreden)
        {
            OnAdressenAnredenCreated(adressenanreden);

            var existingItem = Context.AdressenAnreden
                              .Where(i => i.AnredeCode == adressenanreden.AnredeCode)
                              .FirstOrDefault();

            if (existingItem != null)
            {
               throw new Exception("Item already available");
            }            

            try
            {
                Context.AdressenAnreden.Add(adressenanreden);
                Context.SaveChanges();
            }
            catch
            {
                Context.Entry(adressenanreden).State = EntityState.Detached;
                throw;
            }

            OnAfterAdressenAnredenCreated(adressenanreden);

            return adressenanreden;
        }

        public async Task<STAVerwaltung.Models.dbSTAVerwaltung.AdressenAnreden> CancelAdressenAnredenChanges(STAVerwaltung.Models.dbSTAVerwaltung.AdressenAnreden item)
        {
            var entityToCancel = Context.Entry(item);
            if (entityToCancel.State == EntityState.Modified)
            {
              entityToCancel.CurrentValues.SetValues(entityToCancel.OriginalValues);
              entityToCancel.State = EntityState.Unchanged;
            }

            return item;
        }

        partial void OnAdressenAnredenUpdated(STAVerwaltung.Models.dbSTAVerwaltung.AdressenAnreden item);
        partial void OnAfterAdressenAnredenUpdated(STAVerwaltung.Models.dbSTAVerwaltung.AdressenAnreden item);

        public async Task<STAVerwaltung.Models.dbSTAVerwaltung.AdressenAnreden> UpdateAdressenAnreden(string anredecode, STAVerwaltung.Models.dbSTAVerwaltung.AdressenAnreden adressenanreden)
        {
            OnAdressenAnredenUpdated(adressenanreden);

            var itemToUpdate = Context.AdressenAnreden
                              .Where(i => i.AnredeCode == adressenanreden.AnredeCode)
                              .FirstOrDefault();

            if (itemToUpdate == null)
            {
               throw new Exception("Item no longer available");
            }
                
            var entryToUpdate = Context.Entry(itemToUpdate);
            entryToUpdate.CurrentValues.SetValues(adressenanreden);
            entryToUpdate.State = EntityState.Modified;

            Context.SaveChanges();

            OnAfterAdressenAnredenUpdated(adressenanreden);

            return adressenanreden;
        }

        partial void OnAdressenAnredenDeleted(STAVerwaltung.Models.dbSTAVerwaltung.AdressenAnreden item);
        partial void OnAfterAdressenAnredenDeleted(STAVerwaltung.Models.dbSTAVerwaltung.AdressenAnreden item);

        public async Task<STAVerwaltung.Models.dbSTAVerwaltung.AdressenAnreden> DeleteAdressenAnreden(string anredecode)
        {
            var itemToDelete = Context.AdressenAnreden
                              .Where(i => i.AnredeCode == anredecode)
                              .Include(i => i.Adressen)
                              .FirstOrDefault();

            if (itemToDelete == null)
            {
               throw new Exception("Item no longer available");
            }

            OnAdressenAnredenDeleted(itemToDelete);


            Context.AdressenAnreden.Remove(itemToDelete);

            try
            {
                Context.SaveChanges();
            }
            catch
            {
                Context.Entry(itemToDelete).State = EntityState.Unchanged;
                throw;
            }

            OnAfterAdressenAnredenDeleted(itemToDelete);

            return itemToDelete;
        }
    
        public async Task ExportAdressenArtenToExcel(Query query = null, string fileName = null)
        {
            navigationManager.NavigateTo(query != null ? query.ToUrl($"export/dbstaverwaltung/adressenarten/excel(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')") : $"export/dbstaverwaltung/adressenarten/excel(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')", true);
        }

        public async Task ExportAdressenArtenToCSV(Query query = null, string fileName = null)
        {
            navigationManager.NavigateTo(query != null ? query.ToUrl($"export/dbstaverwaltung/adressenarten/csv(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')") : $"export/dbstaverwaltung/adressenarten/csv(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')", true);
        }

        partial void OnAdressenArtenRead(ref IQueryable<STAVerwaltung.Models.dbSTAVerwaltung.AdressenArten> items);

        public async Task<IQueryable<STAVerwaltung.Models.dbSTAVerwaltung.AdressenArten>> GetAdressenArten(Query query = null)
        {
            var items = Context.AdressenArten.AsQueryable();


            if (query != null)
            {
                if (!string.IsNullOrEmpty(query.Expand))
                {
                    var propertiesToExpand = query.Expand.Split(',');
                    foreach(var p in propertiesToExpand)
                    {
                        items = items.Include(p.Trim());
                    }
                }

                ApplyQuery(ref items, query);
            }

            OnAdressenArtenRead(ref items);

            return await Task.FromResult(items);
        }

        partial void OnAdressenArtenGet(STAVerwaltung.Models.dbSTAVerwaltung.AdressenArten item);
        partial void OnGetAdressenArtenByAdressArt(ref IQueryable<STAVerwaltung.Models.dbSTAVerwaltung.AdressenArten> items);


        public async Task<STAVerwaltung.Models.dbSTAVerwaltung.AdressenArten> GetAdressenArtenByAdressArt(string adressart)
        {
            var items = Context.AdressenArten
                              .AsNoTracking()
                              .Where(i => i.AdressArt == adressart);

 
            OnGetAdressenArtenByAdressArt(ref items);

            var itemToReturn = items.FirstOrDefault();

            OnAdressenArtenGet(itemToReturn);

            return await Task.FromResult(itemToReturn);
        }

        partial void OnAdressenArtenCreated(STAVerwaltung.Models.dbSTAVerwaltung.AdressenArten item);
        partial void OnAfterAdressenArtenCreated(STAVerwaltung.Models.dbSTAVerwaltung.AdressenArten item);

        public async Task<STAVerwaltung.Models.dbSTAVerwaltung.AdressenArten> CreateAdressenArten(STAVerwaltung.Models.dbSTAVerwaltung.AdressenArten adressenarten)
        {
            OnAdressenArtenCreated(adressenarten);

            var existingItem = Context.AdressenArten
                              .Where(i => i.AdressArt == adressenarten.AdressArt)
                              .FirstOrDefault();

            if (existingItem != null)
            {
               throw new Exception("Item already available");
            }            

            try
            {
                Context.AdressenArten.Add(adressenarten);
                Context.SaveChanges();
            }
            catch
            {
                Context.Entry(adressenarten).State = EntityState.Detached;
                throw;
            }

            OnAfterAdressenArtenCreated(adressenarten);

            return adressenarten;
        }

        public async Task<STAVerwaltung.Models.dbSTAVerwaltung.AdressenArten> CancelAdressenArtenChanges(STAVerwaltung.Models.dbSTAVerwaltung.AdressenArten item)
        {
            var entityToCancel = Context.Entry(item);
            if (entityToCancel.State == EntityState.Modified)
            {
              entityToCancel.CurrentValues.SetValues(entityToCancel.OriginalValues);
              entityToCancel.State = EntityState.Unchanged;
            }

            return item;
        }

        partial void OnAdressenArtenUpdated(STAVerwaltung.Models.dbSTAVerwaltung.AdressenArten item);
        partial void OnAfterAdressenArtenUpdated(STAVerwaltung.Models.dbSTAVerwaltung.AdressenArten item);

        public async Task<STAVerwaltung.Models.dbSTAVerwaltung.AdressenArten> UpdateAdressenArten(string adressart, STAVerwaltung.Models.dbSTAVerwaltung.AdressenArten adressenarten)
        {
            OnAdressenArtenUpdated(adressenarten);

            var itemToUpdate = Context.AdressenArten
                              .Where(i => i.AdressArt == adressenarten.AdressArt)
                              .FirstOrDefault();

            if (itemToUpdate == null)
            {
               throw new Exception("Item no longer available");
            }
                
            var entryToUpdate = Context.Entry(itemToUpdate);
            entryToUpdate.CurrentValues.SetValues(adressenarten);
            entryToUpdate.State = EntityState.Modified;

            Context.SaveChanges();

            OnAfterAdressenArtenUpdated(adressenarten);

            return adressenarten;
        }

        partial void OnAdressenArtenDeleted(STAVerwaltung.Models.dbSTAVerwaltung.AdressenArten item);
        partial void OnAfterAdressenArtenDeleted(STAVerwaltung.Models.dbSTAVerwaltung.AdressenArten item);

        public async Task<STAVerwaltung.Models.dbSTAVerwaltung.AdressenArten> DeleteAdressenArten(string adressart)
        {
            var itemToDelete = Context.AdressenArten
                              .Where(i => i.AdressArt == adressart)
                              .Include(i => i.Adressen)
                              .FirstOrDefault();

            if (itemToDelete == null)
            {
               throw new Exception("Item no longer available");
            }

            OnAdressenArtenDeleted(itemToDelete);


            Context.AdressenArten.Remove(itemToDelete);

            try
            {
                Context.SaveChanges();
            }
            catch
            {
                Context.Entry(itemToDelete).State = EntityState.Unchanged;
                throw;
            }

            OnAfterAdressenArtenDeleted(itemToDelete);

            return itemToDelete;
        }
    
        public async Task ExportAdressenEreignisseToExcel(Query query = null, string fileName = null)
        {
            navigationManager.NavigateTo(query != null ? query.ToUrl($"export/dbstaverwaltung/adressenereignisse/excel(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')") : $"export/dbstaverwaltung/adressenereignisse/excel(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')", true);
        }

        public async Task ExportAdressenEreignisseToCSV(Query query = null, string fileName = null)
        {
            navigationManager.NavigateTo(query != null ? query.ToUrl($"export/dbstaverwaltung/adressenereignisse/csv(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')") : $"export/dbstaverwaltung/adressenereignisse/csv(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')", true);
        }

        partial void OnAdressenEreignisseRead(ref IQueryable<STAVerwaltung.Models.dbSTAVerwaltung.AdressenEreignisse> items);

        public async Task<IQueryable<STAVerwaltung.Models.dbSTAVerwaltung.AdressenEreignisse>> GetAdressenEreignisse(Query query = null)
        {
            var items = Context.AdressenEreignisse.AsQueryable();

            items = items.Include(i => i.Adressen);
            items = items.Include(i => i.AdressenEreignisseArten);
            items = items.Include(i => i.Religionen);

            if (query != null)
            {
                if (!string.IsNullOrEmpty(query.Expand))
                {
                    var propertiesToExpand = query.Expand.Split(',');
                    foreach(var p in propertiesToExpand)
                    {
                        items = items.Include(p.Trim());
                    }
                }

                ApplyQuery(ref items, query);
            }

            OnAdressenEreignisseRead(ref items);

            return await Task.FromResult(items);
        }

        partial void OnAdressenEreignisseGet(STAVerwaltung.Models.dbSTAVerwaltung.AdressenEreignisse item);
        partial void OnGetAdressenEreignisseByEreignissNr(ref IQueryable<STAVerwaltung.Models.dbSTAVerwaltung.AdressenEreignisse> items);


        public async Task<STAVerwaltung.Models.dbSTAVerwaltung.AdressenEreignisse> GetAdressenEreignisseByEreignissNr(int ereignissnr)
        {
            var items = Context.AdressenEreignisse
                              .AsNoTracking()
                              .Where(i => i.EreignissNr == ereignissnr);

            items = items.Include(i => i.Adressen);
            items = items.Include(i => i.AdressenEreignisseArten);
            items = items.Include(i => i.Religionen);
 
            OnGetAdressenEreignisseByEreignissNr(ref items);

            var itemToReturn = items.FirstOrDefault();

            OnAdressenEreignisseGet(itemToReturn);

            return await Task.FromResult(itemToReturn);
        }

        partial void OnAdressenEreignisseCreated(STAVerwaltung.Models.dbSTAVerwaltung.AdressenEreignisse item);
        partial void OnAfterAdressenEreignisseCreated(STAVerwaltung.Models.dbSTAVerwaltung.AdressenEreignisse item);

        public async Task<STAVerwaltung.Models.dbSTAVerwaltung.AdressenEreignisse> CreateAdressenEreignisse(STAVerwaltung.Models.dbSTAVerwaltung.AdressenEreignisse adressenereignisse)
        {
            OnAdressenEreignisseCreated(adressenereignisse);

            var existingItem = Context.AdressenEreignisse
                              .Where(i => i.EreignissNr == adressenereignisse.EreignissNr)
                              .FirstOrDefault();

            if (existingItem != null)
            {
               throw new Exception("Item already available");
            }            

            try
            {
                Context.AdressenEreignisse.Add(adressenereignisse);
                Context.SaveChanges();
            }
            catch
            {
                Context.Entry(adressenereignisse).State = EntityState.Detached;
                throw;
            }

            OnAfterAdressenEreignisseCreated(adressenereignisse);

            return adressenereignisse;
        }

        public async Task<STAVerwaltung.Models.dbSTAVerwaltung.AdressenEreignisse> CancelAdressenEreignisseChanges(STAVerwaltung.Models.dbSTAVerwaltung.AdressenEreignisse item)
        {
            var entityToCancel = Context.Entry(item);
            if (entityToCancel.State == EntityState.Modified)
            {
              entityToCancel.CurrentValues.SetValues(entityToCancel.OriginalValues);
              entityToCancel.State = EntityState.Unchanged;
            }

            return item;
        }

        partial void OnAdressenEreignisseUpdated(STAVerwaltung.Models.dbSTAVerwaltung.AdressenEreignisse item);
        partial void OnAfterAdressenEreignisseUpdated(STAVerwaltung.Models.dbSTAVerwaltung.AdressenEreignisse item);

        public async Task<STAVerwaltung.Models.dbSTAVerwaltung.AdressenEreignisse> UpdateAdressenEreignisse(int ereignissnr, STAVerwaltung.Models.dbSTAVerwaltung.AdressenEreignisse adressenereignisse)
        {
            OnAdressenEreignisseUpdated(adressenereignisse);

            var itemToUpdate = Context.AdressenEreignisse
                              .Where(i => i.EreignissNr == adressenereignisse.EreignissNr)
                              .FirstOrDefault();

            if (itemToUpdate == null)
            {
               throw new Exception("Item no longer available");
            }
                
            var entryToUpdate = Context.Entry(itemToUpdate);
            entryToUpdate.CurrentValues.SetValues(adressenereignisse);
            entryToUpdate.State = EntityState.Modified;

            Context.SaveChanges();

            OnAfterAdressenEreignisseUpdated(adressenereignisse);

            return adressenereignisse;
        }

        partial void OnAdressenEreignisseDeleted(STAVerwaltung.Models.dbSTAVerwaltung.AdressenEreignisse item);
        partial void OnAfterAdressenEreignisseDeleted(STAVerwaltung.Models.dbSTAVerwaltung.AdressenEreignisse item);

        public async Task<STAVerwaltung.Models.dbSTAVerwaltung.AdressenEreignisse> DeleteAdressenEreignisse(int ereignissnr)
        {
            var itemToDelete = Context.AdressenEreignisse
                              .Where(i => i.EreignissNr == ereignissnr)
                              .FirstOrDefault();

            if (itemToDelete == null)
            {
               throw new Exception("Item no longer available");
            }

            OnAdressenEreignisseDeleted(itemToDelete);


            Context.AdressenEreignisse.Remove(itemToDelete);

            try
            {
                Context.SaveChanges();
            }
            catch
            {
                Context.Entry(itemToDelete).State = EntityState.Unchanged;
                throw;
            }

            OnAfterAdressenEreignisseDeleted(itemToDelete);

            return itemToDelete;
        }
    
        public async Task ExportAdressenEreignisseArtenToExcel(Query query = null, string fileName = null)
        {
            navigationManager.NavigateTo(query != null ? query.ToUrl($"export/dbstaverwaltung/adressenereignissearten/excel(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')") : $"export/dbstaverwaltung/adressenereignissearten/excel(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')", true);
        }

        public async Task ExportAdressenEreignisseArtenToCSV(Query query = null, string fileName = null)
        {
            navigationManager.NavigateTo(query != null ? query.ToUrl($"export/dbstaverwaltung/adressenereignissearten/csv(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')") : $"export/dbstaverwaltung/adressenereignissearten/csv(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')", true);
        }

        partial void OnAdressenEreignisseArtenRead(ref IQueryable<STAVerwaltung.Models.dbSTAVerwaltung.AdressenEreignisseArten> items);

        public async Task<IQueryable<STAVerwaltung.Models.dbSTAVerwaltung.AdressenEreignisseArten>> GetAdressenEreignisseArten(Query query = null)
        {
            var items = Context.AdressenEreignisseArten.AsQueryable();


            if (query != null)
            {
                if (!string.IsNullOrEmpty(query.Expand))
                {
                    var propertiesToExpand = query.Expand.Split(',');
                    foreach(var p in propertiesToExpand)
                    {
                        items = items.Include(p.Trim());
                    }
                }

                ApplyQuery(ref items, query);
            }

            OnAdressenEreignisseArtenRead(ref items);

            return await Task.FromResult(items);
        }

        partial void OnAdressenEreignisseArtenGet(STAVerwaltung.Models.dbSTAVerwaltung.AdressenEreignisseArten item);
        partial void OnGetAdressenEreignisseArtenByEreignisCode(ref IQueryable<STAVerwaltung.Models.dbSTAVerwaltung.AdressenEreignisseArten> items);


        public async Task<STAVerwaltung.Models.dbSTAVerwaltung.AdressenEreignisseArten> GetAdressenEreignisseArtenByEreignisCode(string ereigniscode)
        {
            var items = Context.AdressenEreignisseArten
                              .AsNoTracking()
                              .Where(i => i.EreignisCode == ereigniscode);

 
            OnGetAdressenEreignisseArtenByEreignisCode(ref items);

            var itemToReturn = items.FirstOrDefault();

            OnAdressenEreignisseArtenGet(itemToReturn);

            return await Task.FromResult(itemToReturn);
        }

        partial void OnAdressenEreignisseArtenCreated(STAVerwaltung.Models.dbSTAVerwaltung.AdressenEreignisseArten item);
        partial void OnAfterAdressenEreignisseArtenCreated(STAVerwaltung.Models.dbSTAVerwaltung.AdressenEreignisseArten item);

        public async Task<STAVerwaltung.Models.dbSTAVerwaltung.AdressenEreignisseArten> CreateAdressenEreignisseArten(STAVerwaltung.Models.dbSTAVerwaltung.AdressenEreignisseArten adressenereignissearten)
        {
            OnAdressenEreignisseArtenCreated(adressenereignissearten);

            var existingItem = Context.AdressenEreignisseArten
                              .Where(i => i.EreignisCode == adressenereignissearten.EreignisCode)
                              .FirstOrDefault();

            if (existingItem != null)
            {
               throw new Exception("Item already available");
            }            

            try
            {
                Context.AdressenEreignisseArten.Add(adressenereignissearten);
                Context.SaveChanges();
            }
            catch
            {
                Context.Entry(adressenereignissearten).State = EntityState.Detached;
                throw;
            }

            OnAfterAdressenEreignisseArtenCreated(adressenereignissearten);

            return adressenereignissearten;
        }

        public async Task<STAVerwaltung.Models.dbSTAVerwaltung.AdressenEreignisseArten> CancelAdressenEreignisseArtenChanges(STAVerwaltung.Models.dbSTAVerwaltung.AdressenEreignisseArten item)
        {
            var entityToCancel = Context.Entry(item);
            if (entityToCancel.State == EntityState.Modified)
            {
              entityToCancel.CurrentValues.SetValues(entityToCancel.OriginalValues);
              entityToCancel.State = EntityState.Unchanged;
            }

            return item;
        }

        partial void OnAdressenEreignisseArtenUpdated(STAVerwaltung.Models.dbSTAVerwaltung.AdressenEreignisseArten item);
        partial void OnAfterAdressenEreignisseArtenUpdated(STAVerwaltung.Models.dbSTAVerwaltung.AdressenEreignisseArten item);

        public async Task<STAVerwaltung.Models.dbSTAVerwaltung.AdressenEreignisseArten> UpdateAdressenEreignisseArten(string ereigniscode, STAVerwaltung.Models.dbSTAVerwaltung.AdressenEreignisseArten adressenereignissearten)
        {
            OnAdressenEreignisseArtenUpdated(adressenereignissearten);

            var itemToUpdate = Context.AdressenEreignisseArten
                              .Where(i => i.EreignisCode == adressenereignissearten.EreignisCode)
                              .FirstOrDefault();

            if (itemToUpdate == null)
            {
               throw new Exception("Item no longer available");
            }
                
            var entryToUpdate = Context.Entry(itemToUpdate);
            entryToUpdate.CurrentValues.SetValues(adressenereignissearten);
            entryToUpdate.State = EntityState.Modified;

            Context.SaveChanges();

            OnAfterAdressenEreignisseArtenUpdated(adressenereignissearten);

            return adressenereignissearten;
        }

        partial void OnAdressenEreignisseArtenDeleted(STAVerwaltung.Models.dbSTAVerwaltung.AdressenEreignisseArten item);
        partial void OnAfterAdressenEreignisseArtenDeleted(STAVerwaltung.Models.dbSTAVerwaltung.AdressenEreignisseArten item);

        public async Task<STAVerwaltung.Models.dbSTAVerwaltung.AdressenEreignisseArten> DeleteAdressenEreignisseArten(string ereigniscode)
        {
            var itemToDelete = Context.AdressenEreignisseArten
                              .Where(i => i.EreignisCode == ereigniscode)
                              .Include(i => i.AdressenEreignisse)
                              .FirstOrDefault();

            if (itemToDelete == null)
            {
               throw new Exception("Item no longer available");
            }

            OnAdressenEreignisseArtenDeleted(itemToDelete);


            Context.AdressenEreignisseArten.Remove(itemToDelete);

            try
            {
                Context.SaveChanges();
            }
            catch
            {
                Context.Entry(itemToDelete).State = EntityState.Unchanged;
                throw;
            }

            OnAfterAdressenEreignisseArtenDeleted(itemToDelete);

            return itemToDelete;
        }
    
        public async Task ExportAdressenFamilienstaendeToExcel(Query query = null, string fileName = null)
        {
            navigationManager.NavigateTo(query != null ? query.ToUrl($"export/dbstaverwaltung/adressenfamilienstaende/excel(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')") : $"export/dbstaverwaltung/adressenfamilienstaende/excel(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')", true);
        }

        public async Task ExportAdressenFamilienstaendeToCSV(Query query = null, string fileName = null)
        {
            navigationManager.NavigateTo(query != null ? query.ToUrl($"export/dbstaverwaltung/adressenfamilienstaende/csv(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')") : $"export/dbstaverwaltung/adressenfamilienstaende/csv(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')", true);
        }

        partial void OnAdressenFamilienstaendeRead(ref IQueryable<STAVerwaltung.Models.dbSTAVerwaltung.AdressenFamilienstaende> items);

        public async Task<IQueryable<STAVerwaltung.Models.dbSTAVerwaltung.AdressenFamilienstaende>> GetAdressenFamilienstaende(Query query = null)
        {
            var items = Context.AdressenFamilienstaende.AsQueryable();


            if (query != null)
            {
                if (!string.IsNullOrEmpty(query.Expand))
                {
                    var propertiesToExpand = query.Expand.Split(',');
                    foreach(var p in propertiesToExpand)
                    {
                        items = items.Include(p.Trim());
                    }
                }

                ApplyQuery(ref items, query);
            }

            OnAdressenFamilienstaendeRead(ref items);

            return await Task.FromResult(items);
        }

        partial void OnAdressenFamilienstaendeGet(STAVerwaltung.Models.dbSTAVerwaltung.AdressenFamilienstaende item);
        partial void OnGetAdressenFamilienstaendeByFamilienstandCode(ref IQueryable<STAVerwaltung.Models.dbSTAVerwaltung.AdressenFamilienstaende> items);


        public async Task<STAVerwaltung.Models.dbSTAVerwaltung.AdressenFamilienstaende> GetAdressenFamilienstaendeByFamilienstandCode(string familienstandcode)
        {
            var items = Context.AdressenFamilienstaende
                              .AsNoTracking()
                              .Where(i => i.FamilienstandCode == familienstandcode);

 
            OnGetAdressenFamilienstaendeByFamilienstandCode(ref items);

            var itemToReturn = items.FirstOrDefault();

            OnAdressenFamilienstaendeGet(itemToReturn);

            return await Task.FromResult(itemToReturn);
        }

        partial void OnAdressenFamilienstaendeCreated(STAVerwaltung.Models.dbSTAVerwaltung.AdressenFamilienstaende item);
        partial void OnAfterAdressenFamilienstaendeCreated(STAVerwaltung.Models.dbSTAVerwaltung.AdressenFamilienstaende item);

        public async Task<STAVerwaltung.Models.dbSTAVerwaltung.AdressenFamilienstaende> CreateAdressenFamilienstaende(STAVerwaltung.Models.dbSTAVerwaltung.AdressenFamilienstaende adressenfamilienstaende)
        {
            OnAdressenFamilienstaendeCreated(adressenfamilienstaende);

            var existingItem = Context.AdressenFamilienstaende
                              .Where(i => i.FamilienstandCode == adressenfamilienstaende.FamilienstandCode)
                              .FirstOrDefault();

            if (existingItem != null)
            {
               throw new Exception("Item already available");
            }            

            try
            {
                Context.AdressenFamilienstaende.Add(adressenfamilienstaende);
                Context.SaveChanges();
            }
            catch
            {
                Context.Entry(adressenfamilienstaende).State = EntityState.Detached;
                throw;
            }

            OnAfterAdressenFamilienstaendeCreated(adressenfamilienstaende);

            return adressenfamilienstaende;
        }

        public async Task<STAVerwaltung.Models.dbSTAVerwaltung.AdressenFamilienstaende> CancelAdressenFamilienstaendeChanges(STAVerwaltung.Models.dbSTAVerwaltung.AdressenFamilienstaende item)
        {
            var entityToCancel = Context.Entry(item);
            if (entityToCancel.State == EntityState.Modified)
            {
              entityToCancel.CurrentValues.SetValues(entityToCancel.OriginalValues);
              entityToCancel.State = EntityState.Unchanged;
            }

            return item;
        }

        partial void OnAdressenFamilienstaendeUpdated(STAVerwaltung.Models.dbSTAVerwaltung.AdressenFamilienstaende item);
        partial void OnAfterAdressenFamilienstaendeUpdated(STAVerwaltung.Models.dbSTAVerwaltung.AdressenFamilienstaende item);

        public async Task<STAVerwaltung.Models.dbSTAVerwaltung.AdressenFamilienstaende> UpdateAdressenFamilienstaende(string familienstandcode, STAVerwaltung.Models.dbSTAVerwaltung.AdressenFamilienstaende adressenfamilienstaende)
        {
            OnAdressenFamilienstaendeUpdated(adressenfamilienstaende);

            var itemToUpdate = Context.AdressenFamilienstaende
                              .Where(i => i.FamilienstandCode == adressenfamilienstaende.FamilienstandCode)
                              .FirstOrDefault();

            if (itemToUpdate == null)
            {
               throw new Exception("Item no longer available");
            }
                
            var entryToUpdate = Context.Entry(itemToUpdate);
            entryToUpdate.CurrentValues.SetValues(adressenfamilienstaende);
            entryToUpdate.State = EntityState.Modified;

            Context.SaveChanges();

            OnAfterAdressenFamilienstaendeUpdated(adressenfamilienstaende);

            return adressenfamilienstaende;
        }

        partial void OnAdressenFamilienstaendeDeleted(STAVerwaltung.Models.dbSTAVerwaltung.AdressenFamilienstaende item);
        partial void OnAfterAdressenFamilienstaendeDeleted(STAVerwaltung.Models.dbSTAVerwaltung.AdressenFamilienstaende item);

        public async Task<STAVerwaltung.Models.dbSTAVerwaltung.AdressenFamilienstaende> DeleteAdressenFamilienstaende(string familienstandcode)
        {
            var itemToDelete = Context.AdressenFamilienstaende
                              .Where(i => i.FamilienstandCode == familienstandcode)
                              .Include(i => i.Adressen)
                              .FirstOrDefault();

            if (itemToDelete == null)
            {
               throw new Exception("Item no longer available");
            }

            OnAdressenFamilienstaendeDeleted(itemToDelete);


            Context.AdressenFamilienstaende.Remove(itemToDelete);

            try
            {
                Context.SaveChanges();
            }
            catch
            {
                Context.Entry(itemToDelete).State = EntityState.Unchanged;
                throw;
            }

            OnAfterAdressenFamilienstaendeDeleted(itemToDelete);

            return itemToDelete;
        }
    
        public async Task ExportAdressenGeschlechterToExcel(Query query = null, string fileName = null)
        {
            navigationManager.NavigateTo(query != null ? query.ToUrl($"export/dbstaverwaltung/adressengeschlechter/excel(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')") : $"export/dbstaverwaltung/adressengeschlechter/excel(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')", true);
        }

        public async Task ExportAdressenGeschlechterToCSV(Query query = null, string fileName = null)
        {
            navigationManager.NavigateTo(query != null ? query.ToUrl($"export/dbstaverwaltung/adressengeschlechter/csv(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')") : $"export/dbstaverwaltung/adressengeschlechter/csv(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')", true);
        }

        partial void OnAdressenGeschlechterRead(ref IQueryable<STAVerwaltung.Models.dbSTAVerwaltung.AdressenGeschlechter> items);

        public async Task<IQueryable<STAVerwaltung.Models.dbSTAVerwaltung.AdressenGeschlechter>> GetAdressenGeschlechter(Query query = null)
        {
            var items = Context.AdressenGeschlechter.AsQueryable();


            if (query != null)
            {
                if (!string.IsNullOrEmpty(query.Expand))
                {
                    var propertiesToExpand = query.Expand.Split(',');
                    foreach(var p in propertiesToExpand)
                    {
                        items = items.Include(p.Trim());
                    }
                }

                ApplyQuery(ref items, query);
            }

            OnAdressenGeschlechterRead(ref items);

            return await Task.FromResult(items);
        }

        partial void OnAdressenGeschlechterGet(STAVerwaltung.Models.dbSTAVerwaltung.AdressenGeschlechter item);
        partial void OnGetAdressenGeschlechterByGeschlecht(ref IQueryable<STAVerwaltung.Models.dbSTAVerwaltung.AdressenGeschlechter> items);


        public async Task<STAVerwaltung.Models.dbSTAVerwaltung.AdressenGeschlechter> GetAdressenGeschlechterByGeschlecht(string geschlecht)
        {
            var items = Context.AdressenGeschlechter
                              .AsNoTracking()
                              .Where(i => i.Geschlecht == geschlecht);

 
            OnGetAdressenGeschlechterByGeschlecht(ref items);

            var itemToReturn = items.FirstOrDefault();

            OnAdressenGeschlechterGet(itemToReturn);

            return await Task.FromResult(itemToReturn);
        }

        partial void OnAdressenGeschlechterCreated(STAVerwaltung.Models.dbSTAVerwaltung.AdressenGeschlechter item);
        partial void OnAfterAdressenGeschlechterCreated(STAVerwaltung.Models.dbSTAVerwaltung.AdressenGeschlechter item);

        public async Task<STAVerwaltung.Models.dbSTAVerwaltung.AdressenGeschlechter> CreateAdressenGeschlechter(STAVerwaltung.Models.dbSTAVerwaltung.AdressenGeschlechter adressengeschlechter)
        {
            OnAdressenGeschlechterCreated(adressengeschlechter);

            var existingItem = Context.AdressenGeschlechter
                              .Where(i => i.Geschlecht == adressengeschlechter.Geschlecht)
                              .FirstOrDefault();

            if (existingItem != null)
            {
               throw new Exception("Item already available");
            }            

            try
            {
                Context.AdressenGeschlechter.Add(adressengeschlechter);
                Context.SaveChanges();
            }
            catch
            {
                Context.Entry(adressengeschlechter).State = EntityState.Detached;
                throw;
            }

            OnAfterAdressenGeschlechterCreated(adressengeschlechter);

            return adressengeschlechter;
        }

        public async Task<STAVerwaltung.Models.dbSTAVerwaltung.AdressenGeschlechter> CancelAdressenGeschlechterChanges(STAVerwaltung.Models.dbSTAVerwaltung.AdressenGeschlechter item)
        {
            var entityToCancel = Context.Entry(item);
            if (entityToCancel.State == EntityState.Modified)
            {
              entityToCancel.CurrentValues.SetValues(entityToCancel.OriginalValues);
              entityToCancel.State = EntityState.Unchanged;
            }

            return item;
        }

        partial void OnAdressenGeschlechterUpdated(STAVerwaltung.Models.dbSTAVerwaltung.AdressenGeschlechter item);
        partial void OnAfterAdressenGeschlechterUpdated(STAVerwaltung.Models.dbSTAVerwaltung.AdressenGeschlechter item);

        public async Task<STAVerwaltung.Models.dbSTAVerwaltung.AdressenGeschlechter> UpdateAdressenGeschlechter(string geschlecht, STAVerwaltung.Models.dbSTAVerwaltung.AdressenGeschlechter adressengeschlechter)
        {
            OnAdressenGeschlechterUpdated(adressengeschlechter);

            var itemToUpdate = Context.AdressenGeschlechter
                              .Where(i => i.Geschlecht == adressengeschlechter.Geschlecht)
                              .FirstOrDefault();

            if (itemToUpdate == null)
            {
               throw new Exception("Item no longer available");
            }
                
            var entryToUpdate = Context.Entry(itemToUpdate);
            entryToUpdate.CurrentValues.SetValues(adressengeschlechter);
            entryToUpdate.State = EntityState.Modified;

            Context.SaveChanges();

            OnAfterAdressenGeschlechterUpdated(adressengeschlechter);

            return adressengeschlechter;
        }

        partial void OnAdressenGeschlechterDeleted(STAVerwaltung.Models.dbSTAVerwaltung.AdressenGeschlechter item);
        partial void OnAfterAdressenGeschlechterDeleted(STAVerwaltung.Models.dbSTAVerwaltung.AdressenGeschlechter item);

        public async Task<STAVerwaltung.Models.dbSTAVerwaltung.AdressenGeschlechter> DeleteAdressenGeschlechter(string geschlecht)
        {
            var itemToDelete = Context.AdressenGeschlechter
                              .Where(i => i.Geschlecht == geschlecht)
                              .Include(i => i.Adressen)
                              .FirstOrDefault();

            if (itemToDelete == null)
            {
               throw new Exception("Item no longer available");
            }

            OnAdressenGeschlechterDeleted(itemToDelete);


            Context.AdressenGeschlechter.Remove(itemToDelete);

            try
            {
                Context.SaveChanges();
            }
            catch
            {
                Context.Entry(itemToDelete).State = EntityState.Unchanged;
                throw;
            }

            OnAfterAdressenGeschlechterDeleted(itemToDelete);

            return itemToDelete;
        }
    
        public async Task ExportAdressenSortierungFamilieToExcel(Query query = null, string fileName = null)
        {
            navigationManager.NavigateTo(query != null ? query.ToUrl($"export/dbstaverwaltung/adressensortierungfamilie/excel(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')") : $"export/dbstaverwaltung/adressensortierungfamilie/excel(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')", true);
        }

        public async Task ExportAdressenSortierungFamilieToCSV(Query query = null, string fileName = null)
        {
            navigationManager.NavigateTo(query != null ? query.ToUrl($"export/dbstaverwaltung/adressensortierungfamilie/csv(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')") : $"export/dbstaverwaltung/adressensortierungfamilie/csv(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')", true);
        }

        partial void OnAdressenSortierungFamilieRead(ref IQueryable<STAVerwaltung.Models.dbSTAVerwaltung.AdressenSortierungFamilie> items);

        public async Task<IQueryable<STAVerwaltung.Models.dbSTAVerwaltung.AdressenSortierungFamilie>> GetAdressenSortierungFamilie(Query query = null)
        {
            var items = Context.AdressenSortierungFamilie.AsQueryable();


            if (query != null)
            {
                if (!string.IsNullOrEmpty(query.Expand))
                {
                    var propertiesToExpand = query.Expand.Split(',');
                    foreach(var p in propertiesToExpand)
                    {
                        items = items.Include(p.Trim());
                    }
                }

                ApplyQuery(ref items, query);
            }

            OnAdressenSortierungFamilieRead(ref items);

            return await Task.FromResult(items);
        }

        partial void OnAdressenSortierungFamilieGet(STAVerwaltung.Models.dbSTAVerwaltung.AdressenSortierungFamilie item);
        partial void OnGetAdressenSortierungFamilieBySortierungFamilie(ref IQueryable<STAVerwaltung.Models.dbSTAVerwaltung.AdressenSortierungFamilie> items);


        public async Task<STAVerwaltung.Models.dbSTAVerwaltung.AdressenSortierungFamilie> GetAdressenSortierungFamilieBySortierungFamilie(int sortierungfamilie)
        {
            var items = Context.AdressenSortierungFamilie
                              .AsNoTracking()
                              .Where(i => i.SortierungFamilie == sortierungfamilie);

 
            OnGetAdressenSortierungFamilieBySortierungFamilie(ref items);

            var itemToReturn = items.FirstOrDefault();

            OnAdressenSortierungFamilieGet(itemToReturn);

            return await Task.FromResult(itemToReturn);
        }

        partial void OnAdressenSortierungFamilieCreated(STAVerwaltung.Models.dbSTAVerwaltung.AdressenSortierungFamilie item);
        partial void OnAfterAdressenSortierungFamilieCreated(STAVerwaltung.Models.dbSTAVerwaltung.AdressenSortierungFamilie item);

        public async Task<STAVerwaltung.Models.dbSTAVerwaltung.AdressenSortierungFamilie> CreateAdressenSortierungFamilie(STAVerwaltung.Models.dbSTAVerwaltung.AdressenSortierungFamilie adressensortierungfamilie)
        {
            OnAdressenSortierungFamilieCreated(adressensortierungfamilie);

            var existingItem = Context.AdressenSortierungFamilie
                              .Where(i => i.SortierungFamilie == adressensortierungfamilie.SortierungFamilie)
                              .FirstOrDefault();

            if (existingItem != null)
            {
               throw new Exception("Item already available");
            }            

            try
            {
                Context.AdressenSortierungFamilie.Add(adressensortierungfamilie);
                Context.SaveChanges();
            }
            catch
            {
                Context.Entry(adressensortierungfamilie).State = EntityState.Detached;
                throw;
            }

            OnAfterAdressenSortierungFamilieCreated(adressensortierungfamilie);

            return adressensortierungfamilie;
        }

        public async Task<STAVerwaltung.Models.dbSTAVerwaltung.AdressenSortierungFamilie> CancelAdressenSortierungFamilieChanges(STAVerwaltung.Models.dbSTAVerwaltung.AdressenSortierungFamilie item)
        {
            var entityToCancel = Context.Entry(item);
            if (entityToCancel.State == EntityState.Modified)
            {
              entityToCancel.CurrentValues.SetValues(entityToCancel.OriginalValues);
              entityToCancel.State = EntityState.Unchanged;
            }

            return item;
        }

        partial void OnAdressenSortierungFamilieUpdated(STAVerwaltung.Models.dbSTAVerwaltung.AdressenSortierungFamilie item);
        partial void OnAfterAdressenSortierungFamilieUpdated(STAVerwaltung.Models.dbSTAVerwaltung.AdressenSortierungFamilie item);

        public async Task<STAVerwaltung.Models.dbSTAVerwaltung.AdressenSortierungFamilie> UpdateAdressenSortierungFamilie(int sortierungfamilie, STAVerwaltung.Models.dbSTAVerwaltung.AdressenSortierungFamilie adressensortierungfamilie)
        {
            OnAdressenSortierungFamilieUpdated(adressensortierungfamilie);

            var itemToUpdate = Context.AdressenSortierungFamilie
                              .Where(i => i.SortierungFamilie == adressensortierungfamilie.SortierungFamilie)
                              .FirstOrDefault();

            if (itemToUpdate == null)
            {
               throw new Exception("Item no longer available");
            }
                
            var entryToUpdate = Context.Entry(itemToUpdate);
            entryToUpdate.CurrentValues.SetValues(adressensortierungfamilie);
            entryToUpdate.State = EntityState.Modified;

            Context.SaveChanges();

            OnAfterAdressenSortierungFamilieUpdated(adressensortierungfamilie);

            return adressensortierungfamilie;
        }

        partial void OnAdressenSortierungFamilieDeleted(STAVerwaltung.Models.dbSTAVerwaltung.AdressenSortierungFamilie item);
        partial void OnAfterAdressenSortierungFamilieDeleted(STAVerwaltung.Models.dbSTAVerwaltung.AdressenSortierungFamilie item);

        public async Task<STAVerwaltung.Models.dbSTAVerwaltung.AdressenSortierungFamilie> DeleteAdressenSortierungFamilie(int sortierungfamilie)
        {
            var itemToDelete = Context.AdressenSortierungFamilie
                              .Where(i => i.SortierungFamilie == sortierungfamilie)
                              .Include(i => i.Adressen)
                              .FirstOrDefault();

            if (itemToDelete == null)
            {
               throw new Exception("Item no longer available");
            }

            OnAdressenSortierungFamilieDeleted(itemToDelete);


            Context.AdressenSortierungFamilie.Remove(itemToDelete);

            try
            {
                Context.SaveChanges();
            }
            catch
            {
                Context.Entry(itemToDelete).State = EntityState.Unchanged;
                throw;
            }

            OnAfterAdressenSortierungFamilieDeleted(itemToDelete);

            return itemToDelete;
        }
    
        public async Task ExportBenutzerToExcel(Query query = null, string fileName = null)
        {
            navigationManager.NavigateTo(query != null ? query.ToUrl($"export/dbstaverwaltung/benutzer/excel(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')") : $"export/dbstaverwaltung/benutzer/excel(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')", true);
        }

        public async Task ExportBenutzerToCSV(Query query = null, string fileName = null)
        {
            navigationManager.NavigateTo(query != null ? query.ToUrl($"export/dbstaverwaltung/benutzer/csv(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')") : $"export/dbstaverwaltung/benutzer/csv(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')", true);
        }

        partial void OnBenutzerRead(ref IQueryable<STAVerwaltung.Models.dbSTAVerwaltung.Benutzer> items);

        public async Task<IQueryable<STAVerwaltung.Models.dbSTAVerwaltung.Benutzer>> GetBenutzer(Query query = null)
        {
            var items = Context.Benutzer.AsQueryable();


            if (query != null)
            {
                if (!string.IsNullOrEmpty(query.Expand))
                {
                    var propertiesToExpand = query.Expand.Split(',');
                    foreach(var p in propertiesToExpand)
                    {
                        items = items.Include(p.Trim());
                    }
                }

                ApplyQuery(ref items, query);
            }

            OnBenutzerRead(ref items);

            return await Task.FromResult(items);
        }

        partial void OnBenutzerGet(STAVerwaltung.Models.dbSTAVerwaltung.Benutzer item);
        partial void OnGetBenutzerByBenutzerNr(ref IQueryable<STAVerwaltung.Models.dbSTAVerwaltung.Benutzer> items);


        public async Task<STAVerwaltung.Models.dbSTAVerwaltung.Benutzer> GetBenutzerByBenutzerNr(int benutzernr)
        {
            var items = Context.Benutzer
                              .AsNoTracking()
                              .Where(i => i.BenutzerNr == benutzernr);

 
            OnGetBenutzerByBenutzerNr(ref items);

            var itemToReturn = items.FirstOrDefault();

            OnBenutzerGet(itemToReturn);

            return await Task.FromResult(itemToReturn);
        }

        partial void OnBenutzerCreated(STAVerwaltung.Models.dbSTAVerwaltung.Benutzer item);
        partial void OnAfterBenutzerCreated(STAVerwaltung.Models.dbSTAVerwaltung.Benutzer item);

        public async Task<STAVerwaltung.Models.dbSTAVerwaltung.Benutzer> CreateBenutzer(STAVerwaltung.Models.dbSTAVerwaltung.Benutzer benutzer)
        {
            OnBenutzerCreated(benutzer);

            var existingItem = Context.Benutzer
                              .Where(i => i.BenutzerNr == benutzer.BenutzerNr)
                              .FirstOrDefault();

            if (existingItem != null)
            {
               throw new Exception("Item already available");
            }            

            try
            {
                Context.Benutzer.Add(benutzer);
                Context.SaveChanges();
            }
            catch
            {
                Context.Entry(benutzer).State = EntityState.Detached;
                throw;
            }

            OnAfterBenutzerCreated(benutzer);

            return benutzer;
        }

        public async Task<STAVerwaltung.Models.dbSTAVerwaltung.Benutzer> CancelBenutzerChanges(STAVerwaltung.Models.dbSTAVerwaltung.Benutzer item)
        {
            var entityToCancel = Context.Entry(item);
            if (entityToCancel.State == EntityState.Modified)
            {
              entityToCancel.CurrentValues.SetValues(entityToCancel.OriginalValues);
              entityToCancel.State = EntityState.Unchanged;
            }

            return item;
        }

        partial void OnBenutzerUpdated(STAVerwaltung.Models.dbSTAVerwaltung.Benutzer item);
        partial void OnAfterBenutzerUpdated(STAVerwaltung.Models.dbSTAVerwaltung.Benutzer item);

        public async Task<STAVerwaltung.Models.dbSTAVerwaltung.Benutzer> UpdateBenutzer(int benutzernr, STAVerwaltung.Models.dbSTAVerwaltung.Benutzer benutzer)
        {
            OnBenutzerUpdated(benutzer);

            var itemToUpdate = Context.Benutzer
                              .Where(i => i.BenutzerNr == benutzer.BenutzerNr)
                              .FirstOrDefault();

            if (itemToUpdate == null)
            {
               throw new Exception("Item no longer available");
            }
                
            var entryToUpdate = Context.Entry(itemToUpdate);
            entryToUpdate.CurrentValues.SetValues(benutzer);
            entryToUpdate.State = EntityState.Modified;

            Context.SaveChanges();

            OnAfterBenutzerUpdated(benutzer);

            return benutzer;
        }

        partial void OnBenutzerDeleted(STAVerwaltung.Models.dbSTAVerwaltung.Benutzer item);
        partial void OnAfterBenutzerDeleted(STAVerwaltung.Models.dbSTAVerwaltung.Benutzer item);

        public async Task<STAVerwaltung.Models.dbSTAVerwaltung.Benutzer> DeleteBenutzer(int benutzernr)
        {
            var itemToDelete = Context.Benutzer
                              .Where(i => i.BenutzerNr == benutzernr)
                              .Include(i => i.BenutzerBerechtigungGemeinden)
                              .Include(i => i.BenutzerBerechtigungOrganisationen)
                              .Include(i => i.BenutzerProtokoll)
                              .FirstOrDefault();

            if (itemToDelete == null)
            {
               throw new Exception("Item no longer available");
            }

            OnBenutzerDeleted(itemToDelete);


            Context.Benutzer.Remove(itemToDelete);

            try
            {
                Context.SaveChanges();
            }
            catch
            {
                Context.Entry(itemToDelete).State = EntityState.Unchanged;
                throw;
            }

            OnAfterBenutzerDeleted(itemToDelete);

            return itemToDelete;
        }
    
        public async Task ExportBenutzerBerechtigungGemeindenToExcel(Query query = null, string fileName = null)
        {
            navigationManager.NavigateTo(query != null ? query.ToUrl($"export/dbstaverwaltung/benutzerberechtigunggemeinden/excel(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')") : $"export/dbstaverwaltung/benutzerberechtigunggemeinden/excel(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')", true);
        }

        public async Task ExportBenutzerBerechtigungGemeindenToCSV(Query query = null, string fileName = null)
        {
            navigationManager.NavigateTo(query != null ? query.ToUrl($"export/dbstaverwaltung/benutzerberechtigunggemeinden/csv(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')") : $"export/dbstaverwaltung/benutzerberechtigunggemeinden/csv(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')", true);
        }

        partial void OnBenutzerBerechtigungGemeindenRead(ref IQueryable<STAVerwaltung.Models.dbSTAVerwaltung.BenutzerBerechtigungGemeinden> items);

        public async Task<IQueryable<STAVerwaltung.Models.dbSTAVerwaltung.BenutzerBerechtigungGemeinden>> GetBenutzerBerechtigungGemeinden(Query query = null)
        {
            var items = Context.BenutzerBerechtigungGemeinden.AsQueryable();

            items = items.Include(i => i.Benutzer);
            items = items.Include(i => i.Gemeinden);

            if (query != null)
            {
                if (!string.IsNullOrEmpty(query.Expand))
                {
                    var propertiesToExpand = query.Expand.Split(',');
                    foreach(var p in propertiesToExpand)
                    {
                        items = items.Include(p.Trim());
                    }
                }

                ApplyQuery(ref items, query);
            }

            OnBenutzerBerechtigungGemeindenRead(ref items);

            return await Task.FromResult(items);
        }

        partial void OnBenutzerBerechtigungGemeindenGet(STAVerwaltung.Models.dbSTAVerwaltung.BenutzerBerechtigungGemeinden item);
        partial void OnGetBenutzerBerechtigungGemeindenByBenutzerBerechtigungGemeindenNr(ref IQueryable<STAVerwaltung.Models.dbSTAVerwaltung.BenutzerBerechtigungGemeinden> items);


        public async Task<STAVerwaltung.Models.dbSTAVerwaltung.BenutzerBerechtigungGemeinden> GetBenutzerBerechtigungGemeindenByBenutzerBerechtigungGemeindenNr(int benutzerberechtigunggemeindennr)
        {
            var items = Context.BenutzerBerechtigungGemeinden
                              .AsNoTracking()
                              .Where(i => i.BenutzerBerechtigungGemeindenNr == benutzerberechtigunggemeindennr);

            items = items.Include(i => i.Benutzer);
            items = items.Include(i => i.Gemeinden);
 
            OnGetBenutzerBerechtigungGemeindenByBenutzerBerechtigungGemeindenNr(ref items);

            var itemToReturn = items.FirstOrDefault();

            OnBenutzerBerechtigungGemeindenGet(itemToReturn);

            return await Task.FromResult(itemToReturn);
        }

        partial void OnBenutzerBerechtigungGemeindenCreated(STAVerwaltung.Models.dbSTAVerwaltung.BenutzerBerechtigungGemeinden item);
        partial void OnAfterBenutzerBerechtigungGemeindenCreated(STAVerwaltung.Models.dbSTAVerwaltung.BenutzerBerechtigungGemeinden item);

        public async Task<STAVerwaltung.Models.dbSTAVerwaltung.BenutzerBerechtigungGemeinden> CreateBenutzerBerechtigungGemeinden(STAVerwaltung.Models.dbSTAVerwaltung.BenutzerBerechtigungGemeinden benutzerberechtigunggemeinden)
        {
            OnBenutzerBerechtigungGemeindenCreated(benutzerberechtigunggemeinden);

            var existingItem = Context.BenutzerBerechtigungGemeinden
                              .Where(i => i.BenutzerBerechtigungGemeindenNr == benutzerberechtigunggemeinden.BenutzerBerechtigungGemeindenNr)
                              .FirstOrDefault();

            if (existingItem != null)
            {
               throw new Exception("Item already available");
            }            

            try
            {
                Context.BenutzerBerechtigungGemeinden.Add(benutzerberechtigunggemeinden);
                Context.SaveChanges();
            }
            catch
            {
                Context.Entry(benutzerberechtigunggemeinden).State = EntityState.Detached;
                throw;
            }

            OnAfterBenutzerBerechtigungGemeindenCreated(benutzerberechtigunggemeinden);

            return benutzerberechtigunggemeinden;
        }

        public async Task<STAVerwaltung.Models.dbSTAVerwaltung.BenutzerBerechtigungGemeinden> CancelBenutzerBerechtigungGemeindenChanges(STAVerwaltung.Models.dbSTAVerwaltung.BenutzerBerechtigungGemeinden item)
        {
            var entityToCancel = Context.Entry(item);
            if (entityToCancel.State == EntityState.Modified)
            {
              entityToCancel.CurrentValues.SetValues(entityToCancel.OriginalValues);
              entityToCancel.State = EntityState.Unchanged;
            }

            return item;
        }

        partial void OnBenutzerBerechtigungGemeindenUpdated(STAVerwaltung.Models.dbSTAVerwaltung.BenutzerBerechtigungGemeinden item);
        partial void OnAfterBenutzerBerechtigungGemeindenUpdated(STAVerwaltung.Models.dbSTAVerwaltung.BenutzerBerechtigungGemeinden item);

        public async Task<STAVerwaltung.Models.dbSTAVerwaltung.BenutzerBerechtigungGemeinden> UpdateBenutzerBerechtigungGemeinden(int benutzerberechtigunggemeindennr, STAVerwaltung.Models.dbSTAVerwaltung.BenutzerBerechtigungGemeinden benutzerberechtigunggemeinden)
        {
            OnBenutzerBerechtigungGemeindenUpdated(benutzerberechtigunggemeinden);

            var itemToUpdate = Context.BenutzerBerechtigungGemeinden
                              .Where(i => i.BenutzerBerechtigungGemeindenNr == benutzerberechtigunggemeinden.BenutzerBerechtigungGemeindenNr)
                              .FirstOrDefault();

            if (itemToUpdate == null)
            {
               throw new Exception("Item no longer available");
            }
                
            var entryToUpdate = Context.Entry(itemToUpdate);
            entryToUpdate.CurrentValues.SetValues(benutzerberechtigunggemeinden);
            entryToUpdate.State = EntityState.Modified;

            Context.SaveChanges();

            OnAfterBenutzerBerechtigungGemeindenUpdated(benutzerberechtigunggemeinden);

            return benutzerberechtigunggemeinden;
        }

        partial void OnBenutzerBerechtigungGemeindenDeleted(STAVerwaltung.Models.dbSTAVerwaltung.BenutzerBerechtigungGemeinden item);
        partial void OnAfterBenutzerBerechtigungGemeindenDeleted(STAVerwaltung.Models.dbSTAVerwaltung.BenutzerBerechtigungGemeinden item);

        public async Task<STAVerwaltung.Models.dbSTAVerwaltung.BenutzerBerechtigungGemeinden> DeleteBenutzerBerechtigungGemeinden(int benutzerberechtigunggemeindennr)
        {
            var itemToDelete = Context.BenutzerBerechtigungGemeinden
                              .Where(i => i.BenutzerBerechtigungGemeindenNr == benutzerberechtigunggemeindennr)
                              .FirstOrDefault();

            if (itemToDelete == null)
            {
               throw new Exception("Item no longer available");
            }

            OnBenutzerBerechtigungGemeindenDeleted(itemToDelete);


            Context.BenutzerBerechtigungGemeinden.Remove(itemToDelete);

            try
            {
                Context.SaveChanges();
            }
            catch
            {
                Context.Entry(itemToDelete).State = EntityState.Unchanged;
                throw;
            }

            OnAfterBenutzerBerechtigungGemeindenDeleted(itemToDelete);

            return itemToDelete;
        }
    
        public async Task ExportBenutzerBerechtigungOrganisationenToExcel(Query query = null, string fileName = null)
        {
            navigationManager.NavigateTo(query != null ? query.ToUrl($"export/dbstaverwaltung/benutzerberechtigungorganisationen/excel(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')") : $"export/dbstaverwaltung/benutzerberechtigungorganisationen/excel(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')", true);
        }

        public async Task ExportBenutzerBerechtigungOrganisationenToCSV(Query query = null, string fileName = null)
        {
            navigationManager.NavigateTo(query != null ? query.ToUrl($"export/dbstaverwaltung/benutzerberechtigungorganisationen/csv(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')") : $"export/dbstaverwaltung/benutzerberechtigungorganisationen/csv(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')", true);
        }

        partial void OnBenutzerBerechtigungOrganisationenRead(ref IQueryable<STAVerwaltung.Models.dbSTAVerwaltung.BenutzerBerechtigungOrganisationen> items);

        public async Task<IQueryable<STAVerwaltung.Models.dbSTAVerwaltung.BenutzerBerechtigungOrganisationen>> GetBenutzerBerechtigungOrganisationen(Query query = null)
        {
            var items = Context.BenutzerBerechtigungOrganisationen.AsQueryable();

            items = items.Include(i => i.Benutzer);
            items = items.Include(i => i.Organisationen);

            if (query != null)
            {
                if (!string.IsNullOrEmpty(query.Expand))
                {
                    var propertiesToExpand = query.Expand.Split(',');
                    foreach(var p in propertiesToExpand)
                    {
                        items = items.Include(p.Trim());
                    }
                }

                ApplyQuery(ref items, query);
            }

            OnBenutzerBerechtigungOrganisationenRead(ref items);

            return await Task.FromResult(items);
        }

        partial void OnBenutzerBerechtigungOrganisationenGet(STAVerwaltung.Models.dbSTAVerwaltung.BenutzerBerechtigungOrganisationen item);
        partial void OnGetBenutzerBerechtigungOrganisationenByBenutzerBerechtigungOrganisationenNr(ref IQueryable<STAVerwaltung.Models.dbSTAVerwaltung.BenutzerBerechtigungOrganisationen> items);


        public async Task<STAVerwaltung.Models.dbSTAVerwaltung.BenutzerBerechtigungOrganisationen> GetBenutzerBerechtigungOrganisationenByBenutzerBerechtigungOrganisationenNr(int benutzerberechtigungorganisationennr)
        {
            var items = Context.BenutzerBerechtigungOrganisationen
                              .AsNoTracking()
                              .Where(i => i.BenutzerBerechtigungOrganisationenNr == benutzerberechtigungorganisationennr);

            items = items.Include(i => i.Benutzer);
            items = items.Include(i => i.Organisationen);
 
            OnGetBenutzerBerechtigungOrganisationenByBenutzerBerechtigungOrganisationenNr(ref items);

            var itemToReturn = items.FirstOrDefault();

            OnBenutzerBerechtigungOrganisationenGet(itemToReturn);

            return await Task.FromResult(itemToReturn);
        }

        partial void OnBenutzerBerechtigungOrganisationenCreated(STAVerwaltung.Models.dbSTAVerwaltung.BenutzerBerechtigungOrganisationen item);
        partial void OnAfterBenutzerBerechtigungOrganisationenCreated(STAVerwaltung.Models.dbSTAVerwaltung.BenutzerBerechtigungOrganisationen item);

        public async Task<STAVerwaltung.Models.dbSTAVerwaltung.BenutzerBerechtigungOrganisationen> CreateBenutzerBerechtigungOrganisationen(STAVerwaltung.Models.dbSTAVerwaltung.BenutzerBerechtigungOrganisationen benutzerberechtigungorganisationen)
        {
            OnBenutzerBerechtigungOrganisationenCreated(benutzerberechtigungorganisationen);

            var existingItem = Context.BenutzerBerechtigungOrganisationen
                              .Where(i => i.BenutzerBerechtigungOrganisationenNr == benutzerberechtigungorganisationen.BenutzerBerechtigungOrganisationenNr)
                              .FirstOrDefault();

            if (existingItem != null)
            {
               throw new Exception("Item already available");
            }            

            try
            {
                Context.BenutzerBerechtigungOrganisationen.Add(benutzerberechtigungorganisationen);
                Context.SaveChanges();
            }
            catch
            {
                Context.Entry(benutzerberechtigungorganisationen).State = EntityState.Detached;
                throw;
            }

            OnAfterBenutzerBerechtigungOrganisationenCreated(benutzerberechtigungorganisationen);

            return benutzerberechtigungorganisationen;
        }

        public async Task<STAVerwaltung.Models.dbSTAVerwaltung.BenutzerBerechtigungOrganisationen> CancelBenutzerBerechtigungOrganisationenChanges(STAVerwaltung.Models.dbSTAVerwaltung.BenutzerBerechtigungOrganisationen item)
        {
            var entityToCancel = Context.Entry(item);
            if (entityToCancel.State == EntityState.Modified)
            {
              entityToCancel.CurrentValues.SetValues(entityToCancel.OriginalValues);
              entityToCancel.State = EntityState.Unchanged;
            }

            return item;
        }

        partial void OnBenutzerBerechtigungOrganisationenUpdated(STAVerwaltung.Models.dbSTAVerwaltung.BenutzerBerechtigungOrganisationen item);
        partial void OnAfterBenutzerBerechtigungOrganisationenUpdated(STAVerwaltung.Models.dbSTAVerwaltung.BenutzerBerechtigungOrganisationen item);

        public async Task<STAVerwaltung.Models.dbSTAVerwaltung.BenutzerBerechtigungOrganisationen> UpdateBenutzerBerechtigungOrganisationen(int benutzerberechtigungorganisationennr, STAVerwaltung.Models.dbSTAVerwaltung.BenutzerBerechtigungOrganisationen benutzerberechtigungorganisationen)
        {
            OnBenutzerBerechtigungOrganisationenUpdated(benutzerberechtigungorganisationen);

            var itemToUpdate = Context.BenutzerBerechtigungOrganisationen
                              .Where(i => i.BenutzerBerechtigungOrganisationenNr == benutzerberechtigungorganisationen.BenutzerBerechtigungOrganisationenNr)
                              .FirstOrDefault();

            if (itemToUpdate == null)
            {
               throw new Exception("Item no longer available");
            }
                
            var entryToUpdate = Context.Entry(itemToUpdate);
            entryToUpdate.CurrentValues.SetValues(benutzerberechtigungorganisationen);
            entryToUpdate.State = EntityState.Modified;

            Context.SaveChanges();

            OnAfterBenutzerBerechtigungOrganisationenUpdated(benutzerberechtigungorganisationen);

            return benutzerberechtigungorganisationen;
        }

        partial void OnBenutzerBerechtigungOrganisationenDeleted(STAVerwaltung.Models.dbSTAVerwaltung.BenutzerBerechtigungOrganisationen item);
        partial void OnAfterBenutzerBerechtigungOrganisationenDeleted(STAVerwaltung.Models.dbSTAVerwaltung.BenutzerBerechtigungOrganisationen item);

        public async Task<STAVerwaltung.Models.dbSTAVerwaltung.BenutzerBerechtigungOrganisationen> DeleteBenutzerBerechtigungOrganisationen(int benutzerberechtigungorganisationennr)
        {
            var itemToDelete = Context.BenutzerBerechtigungOrganisationen
                              .Where(i => i.BenutzerBerechtigungOrganisationenNr == benutzerberechtigungorganisationennr)
                              .FirstOrDefault();

            if (itemToDelete == null)
            {
               throw new Exception("Item no longer available");
            }

            OnBenutzerBerechtigungOrganisationenDeleted(itemToDelete);


            Context.BenutzerBerechtigungOrganisationen.Remove(itemToDelete);

            try
            {
                Context.SaveChanges();
            }
            catch
            {
                Context.Entry(itemToDelete).State = EntityState.Unchanged;
                throw;
            }

            OnAfterBenutzerBerechtigungOrganisationenDeleted(itemToDelete);

            return itemToDelete;
        }
    
        public async Task ExportBenutzerProtokollToExcel(Query query = null, string fileName = null)
        {
            navigationManager.NavigateTo(query != null ? query.ToUrl($"export/dbstaverwaltung/benutzerprotokoll/excel(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')") : $"export/dbstaverwaltung/benutzerprotokoll/excel(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')", true);
        }

        public async Task ExportBenutzerProtokollToCSV(Query query = null, string fileName = null)
        {
            navigationManager.NavigateTo(query != null ? query.ToUrl($"export/dbstaverwaltung/benutzerprotokoll/csv(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')") : $"export/dbstaverwaltung/benutzerprotokoll/csv(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')", true);
        }

        partial void OnBenutzerProtokollRead(ref IQueryable<STAVerwaltung.Models.dbSTAVerwaltung.BenutzerProtokoll> items);

        public async Task<IQueryable<STAVerwaltung.Models.dbSTAVerwaltung.BenutzerProtokoll>> GetBenutzerProtokoll(Query query = null)
        {
            var items = Context.BenutzerProtokoll.AsQueryable();

            items = items.Include(i => i.Benutzer);

            if (query != null)
            {
                if (!string.IsNullOrEmpty(query.Expand))
                {
                    var propertiesToExpand = query.Expand.Split(',');
                    foreach(var p in propertiesToExpand)
                    {
                        items = items.Include(p.Trim());
                    }
                }

                ApplyQuery(ref items, query);
            }

            OnBenutzerProtokollRead(ref items);

            return await Task.FromResult(items);
        }

        partial void OnBenutzerProtokollGet(STAVerwaltung.Models.dbSTAVerwaltung.BenutzerProtokoll item);
        partial void OnGetBenutzerProtokollByBenutzerProtokollNr(ref IQueryable<STAVerwaltung.Models.dbSTAVerwaltung.BenutzerProtokoll> items);


        public async Task<STAVerwaltung.Models.dbSTAVerwaltung.BenutzerProtokoll> GetBenutzerProtokollByBenutzerProtokollNr(int benutzerprotokollnr)
        {
            var items = Context.BenutzerProtokoll
                              .AsNoTracking()
                              .Where(i => i.BenutzerProtokollNr == benutzerprotokollnr);

            items = items.Include(i => i.Benutzer);
 
            OnGetBenutzerProtokollByBenutzerProtokollNr(ref items);

            var itemToReturn = items.FirstOrDefault();

            OnBenutzerProtokollGet(itemToReturn);

            return await Task.FromResult(itemToReturn);
        }

        partial void OnBenutzerProtokollCreated(STAVerwaltung.Models.dbSTAVerwaltung.BenutzerProtokoll item);
        partial void OnAfterBenutzerProtokollCreated(STAVerwaltung.Models.dbSTAVerwaltung.BenutzerProtokoll item);

        public async Task<STAVerwaltung.Models.dbSTAVerwaltung.BenutzerProtokoll> CreateBenutzerProtokoll(STAVerwaltung.Models.dbSTAVerwaltung.BenutzerProtokoll benutzerprotokoll)
        {
            OnBenutzerProtokollCreated(benutzerprotokoll);

            var existingItem = Context.BenutzerProtokoll
                              .Where(i => i.BenutzerProtokollNr == benutzerprotokoll.BenutzerProtokollNr)
                              .FirstOrDefault();

            if (existingItem != null)
            {
               throw new Exception("Item already available");
            }            

            try
            {
                Context.BenutzerProtokoll.Add(benutzerprotokoll);
                Context.SaveChanges();
            }
            catch
            {
                Context.Entry(benutzerprotokoll).State = EntityState.Detached;
                throw;
            }

            OnAfterBenutzerProtokollCreated(benutzerprotokoll);

            return benutzerprotokoll;
        }

        public async Task<STAVerwaltung.Models.dbSTAVerwaltung.BenutzerProtokoll> CancelBenutzerProtokollChanges(STAVerwaltung.Models.dbSTAVerwaltung.BenutzerProtokoll item)
        {
            var entityToCancel = Context.Entry(item);
            if (entityToCancel.State == EntityState.Modified)
            {
              entityToCancel.CurrentValues.SetValues(entityToCancel.OriginalValues);
              entityToCancel.State = EntityState.Unchanged;
            }

            return item;
        }

        partial void OnBenutzerProtokollUpdated(STAVerwaltung.Models.dbSTAVerwaltung.BenutzerProtokoll item);
        partial void OnAfterBenutzerProtokollUpdated(STAVerwaltung.Models.dbSTAVerwaltung.BenutzerProtokoll item);

        public async Task<STAVerwaltung.Models.dbSTAVerwaltung.BenutzerProtokoll> UpdateBenutzerProtokoll(int benutzerprotokollnr, STAVerwaltung.Models.dbSTAVerwaltung.BenutzerProtokoll benutzerprotokoll)
        {
            OnBenutzerProtokollUpdated(benutzerprotokoll);

            var itemToUpdate = Context.BenutzerProtokoll
                              .Where(i => i.BenutzerProtokollNr == benutzerprotokoll.BenutzerProtokollNr)
                              .FirstOrDefault();

            if (itemToUpdate == null)
            {
               throw new Exception("Item no longer available");
            }
                
            var entryToUpdate = Context.Entry(itemToUpdate);
            entryToUpdate.CurrentValues.SetValues(benutzerprotokoll);
            entryToUpdate.State = EntityState.Modified;

            Context.SaveChanges();

            OnAfterBenutzerProtokollUpdated(benutzerprotokoll);

            return benutzerprotokoll;
        }

        partial void OnBenutzerProtokollDeleted(STAVerwaltung.Models.dbSTAVerwaltung.BenutzerProtokoll item);
        partial void OnAfterBenutzerProtokollDeleted(STAVerwaltung.Models.dbSTAVerwaltung.BenutzerProtokoll item);

        public async Task<STAVerwaltung.Models.dbSTAVerwaltung.BenutzerProtokoll> DeleteBenutzerProtokoll(int benutzerprotokollnr)
        {
            var itemToDelete = Context.BenutzerProtokoll
                              .Where(i => i.BenutzerProtokollNr == benutzerprotokollnr)
                              .FirstOrDefault();

            if (itemToDelete == null)
            {
               throw new Exception("Item no longer available");
            }

            OnBenutzerProtokollDeleted(itemToDelete);


            Context.BenutzerProtokoll.Remove(itemToDelete);

            try
            {
                Context.SaveChanges();
            }
            catch
            {
                Context.Entry(itemToDelete).State = EntityState.Unchanged;
                throw;
            }

            OnAfterBenutzerProtokollDeleted(itemToDelete);

            return itemToDelete;
        }
    
        public async Task ExportBundeslaenderToExcel(Query query = null, string fileName = null)
        {
            navigationManager.NavigateTo(query != null ? query.ToUrl($"export/dbstaverwaltung/bundeslaender/excel(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')") : $"export/dbstaverwaltung/bundeslaender/excel(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')", true);
        }

        public async Task ExportBundeslaenderToCSV(Query query = null, string fileName = null)
        {
            navigationManager.NavigateTo(query != null ? query.ToUrl($"export/dbstaverwaltung/bundeslaender/csv(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')") : $"export/dbstaverwaltung/bundeslaender/csv(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')", true);
        }

        partial void OnBundeslaenderRead(ref IQueryable<STAVerwaltung.Models.dbSTAVerwaltung.Bundeslaender> items);

        public async Task<IQueryable<STAVerwaltung.Models.dbSTAVerwaltung.Bundeslaender>> GetBundeslaender(Query query = null)
        {
            var items = Context.Bundeslaender.AsQueryable();

            items = items.Include(i => i.LKZ1);

            if (query != null)
            {
                if (!string.IsNullOrEmpty(query.Expand))
                {
                    var propertiesToExpand = query.Expand.Split(',');
                    foreach(var p in propertiesToExpand)
                    {
                        items = items.Include(p.Trim());
                    }
                }

                ApplyQuery(ref items, query);
            }

            OnBundeslaenderRead(ref items);

            return await Task.FromResult(items);
        }

        partial void OnBundeslaenderGet(STAVerwaltung.Models.dbSTAVerwaltung.Bundeslaender item);
        partial void OnGetBundeslaenderByBundeslandCode(ref IQueryable<STAVerwaltung.Models.dbSTAVerwaltung.Bundeslaender> items);


        public async Task<STAVerwaltung.Models.dbSTAVerwaltung.Bundeslaender> GetBundeslaenderByBundeslandCode(string bundeslandcode)
        {
            var items = Context.Bundeslaender
                              .AsNoTracking()
                              .Where(i => i.BundeslandCode == bundeslandcode);

            items = items.Include(i => i.LKZ1);
 
            OnGetBundeslaenderByBundeslandCode(ref items);

            var itemToReturn = items.FirstOrDefault();

            OnBundeslaenderGet(itemToReturn);

            return await Task.FromResult(itemToReturn);
        }

        partial void OnBundeslaenderCreated(STAVerwaltung.Models.dbSTAVerwaltung.Bundeslaender item);
        partial void OnAfterBundeslaenderCreated(STAVerwaltung.Models.dbSTAVerwaltung.Bundeslaender item);

        public async Task<STAVerwaltung.Models.dbSTAVerwaltung.Bundeslaender> CreateBundeslaender(STAVerwaltung.Models.dbSTAVerwaltung.Bundeslaender bundeslaender)
        {
            OnBundeslaenderCreated(bundeslaender);

            var existingItem = Context.Bundeslaender
                              .Where(i => i.BundeslandCode == bundeslaender.BundeslandCode)
                              .FirstOrDefault();

            if (existingItem != null)
            {
               throw new Exception("Item already available");
            }            

            try
            {
                Context.Bundeslaender.Add(bundeslaender);
                Context.SaveChanges();
            }
            catch
            {
                Context.Entry(bundeslaender).State = EntityState.Detached;
                throw;
            }

            OnAfterBundeslaenderCreated(bundeslaender);

            return bundeslaender;
        }

        public async Task<STAVerwaltung.Models.dbSTAVerwaltung.Bundeslaender> CancelBundeslaenderChanges(STAVerwaltung.Models.dbSTAVerwaltung.Bundeslaender item)
        {
            var entityToCancel = Context.Entry(item);
            if (entityToCancel.State == EntityState.Modified)
            {
              entityToCancel.CurrentValues.SetValues(entityToCancel.OriginalValues);
              entityToCancel.State = EntityState.Unchanged;
            }

            return item;
        }

        partial void OnBundeslaenderUpdated(STAVerwaltung.Models.dbSTAVerwaltung.Bundeslaender item);
        partial void OnAfterBundeslaenderUpdated(STAVerwaltung.Models.dbSTAVerwaltung.Bundeslaender item);

        public async Task<STAVerwaltung.Models.dbSTAVerwaltung.Bundeslaender> UpdateBundeslaender(string bundeslandcode, STAVerwaltung.Models.dbSTAVerwaltung.Bundeslaender bundeslaender)
        {
            OnBundeslaenderUpdated(bundeslaender);

            var itemToUpdate = Context.Bundeslaender
                              .Where(i => i.BundeslandCode == bundeslaender.BundeslandCode)
                              .FirstOrDefault();

            if (itemToUpdate == null)
            {
               throw new Exception("Item no longer available");
            }
                
            var entryToUpdate = Context.Entry(itemToUpdate);
            entryToUpdate.CurrentValues.SetValues(bundeslaender);
            entryToUpdate.State = EntityState.Modified;

            Context.SaveChanges();

            OnAfterBundeslaenderUpdated(bundeslaender);

            return bundeslaender;
        }

        partial void OnBundeslaenderDeleted(STAVerwaltung.Models.dbSTAVerwaltung.Bundeslaender item);
        partial void OnAfterBundeslaenderDeleted(STAVerwaltung.Models.dbSTAVerwaltung.Bundeslaender item);

        public async Task<STAVerwaltung.Models.dbSTAVerwaltung.Bundeslaender> DeleteBundeslaender(string bundeslandcode)
        {
            var itemToDelete = Context.Bundeslaender
                              .Where(i => i.BundeslandCode == bundeslandcode)
                              .Include(i => i.Adressen)
                              .Include(i => i.Gemeinden)
                              .FirstOrDefault();

            if (itemToDelete == null)
            {
               throw new Exception("Item no longer available");
            }

            OnBundeslaenderDeleted(itemToDelete);


            Context.Bundeslaender.Remove(itemToDelete);

            try
            {
                Context.SaveChanges();
            }
            catch
            {
                Context.Entry(itemToDelete).State = EntityState.Unchanged;
                throw;
            }

            OnAfterBundeslaenderDeleted(itemToDelete);

            return itemToDelete;
        }
    
        public async Task ExportGemeindenToExcel(Query query = null, string fileName = null)
        {
            navigationManager.NavigateTo(query != null ? query.ToUrl($"export/dbstaverwaltung/gemeinden/excel(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')") : $"export/dbstaverwaltung/gemeinden/excel(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')", true);
        }

        public async Task ExportGemeindenToCSV(Query query = null, string fileName = null)
        {
            navigationManager.NavigateTo(query != null ? query.ToUrl($"export/dbstaverwaltung/gemeinden/csv(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')") : $"export/dbstaverwaltung/gemeinden/csv(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')", true);
        }

        partial void OnGemeindenRead(ref IQueryable<STAVerwaltung.Models.dbSTAVerwaltung.Gemeinden> items);

        public async Task<IQueryable<STAVerwaltung.Models.dbSTAVerwaltung.Gemeinden>> GetGemeinden(Query query = null)
        {
            var items = Context.Gemeinden.AsQueryable();

            items = items.Include(i => i.Bundeslaender);
            items = items.Include(i => i.GemeindenArten);
            items = items.Include(i => i.LKZ1);
            items = items.Include(i => i.Organisationen);

            if (query != null)
            {
                if (!string.IsNullOrEmpty(query.Expand))
                {
                    var propertiesToExpand = query.Expand.Split(',');
                    foreach(var p in propertiesToExpand)
                    {
                        items = items.Include(p.Trim());
                    }
                }

                ApplyQuery(ref items, query);
            }

            OnGemeindenRead(ref items);

            return await Task.FromResult(items);
        }

        partial void OnGemeindenGet(STAVerwaltung.Models.dbSTAVerwaltung.Gemeinden item);
        partial void OnGetGemeindenByGemeindeNr(ref IQueryable<STAVerwaltung.Models.dbSTAVerwaltung.Gemeinden> items);


        public async Task<STAVerwaltung.Models.dbSTAVerwaltung.Gemeinden> GetGemeindenByGemeindeNr(int gemeindenr)
        {
            var items = Context.Gemeinden
                              .AsNoTracking()
                              .Where(i => i.GemeindeNr == gemeindenr);

            items = items.Include(i => i.Bundeslaender);
            items = items.Include(i => i.GemeindenArten);
            items = items.Include(i => i.LKZ1);
            items = items.Include(i => i.Organisationen);
 
            OnGetGemeindenByGemeindeNr(ref items);

            var itemToReturn = items.FirstOrDefault();

            OnGemeindenGet(itemToReturn);

            return await Task.FromResult(itemToReturn);
        }

        partial void OnGemeindenCreated(STAVerwaltung.Models.dbSTAVerwaltung.Gemeinden item);
        partial void OnAfterGemeindenCreated(STAVerwaltung.Models.dbSTAVerwaltung.Gemeinden item);

        public async Task<STAVerwaltung.Models.dbSTAVerwaltung.Gemeinden> CreateGemeinden(STAVerwaltung.Models.dbSTAVerwaltung.Gemeinden gemeinden)
        {
            OnGemeindenCreated(gemeinden);

            var existingItem = Context.Gemeinden
                              .Where(i => i.GemeindeNr == gemeinden.GemeindeNr)
                              .FirstOrDefault();

            if (existingItem != null)
            {
               throw new Exception("Item already available");
            }            

            try
            {
                Context.Gemeinden.Add(gemeinden);
                Context.SaveChanges();
            }
            catch
            {
                Context.Entry(gemeinden).State = EntityState.Detached;
                throw;
            }

            OnAfterGemeindenCreated(gemeinden);

            return gemeinden;
        }

        public async Task<STAVerwaltung.Models.dbSTAVerwaltung.Gemeinden> CancelGemeindenChanges(STAVerwaltung.Models.dbSTAVerwaltung.Gemeinden item)
        {
            var entityToCancel = Context.Entry(item);
            if (entityToCancel.State == EntityState.Modified)
            {
              entityToCancel.CurrentValues.SetValues(entityToCancel.OriginalValues);
              entityToCancel.State = EntityState.Unchanged;
            }

            return item;
        }

        partial void OnGemeindenUpdated(STAVerwaltung.Models.dbSTAVerwaltung.Gemeinden item);
        partial void OnAfterGemeindenUpdated(STAVerwaltung.Models.dbSTAVerwaltung.Gemeinden item);

        public async Task<STAVerwaltung.Models.dbSTAVerwaltung.Gemeinden> UpdateGemeinden(int gemeindenr, STAVerwaltung.Models.dbSTAVerwaltung.Gemeinden gemeinden)
        {
            OnGemeindenUpdated(gemeinden);

            var itemToUpdate = Context.Gemeinden
                              .Where(i => i.GemeindeNr == gemeinden.GemeindeNr)
                              .FirstOrDefault();

            if (itemToUpdate == null)
            {
               throw new Exception("Item no longer available");
            }
                
            var entryToUpdate = Context.Entry(itemToUpdate);
            entryToUpdate.CurrentValues.SetValues(gemeinden);
            entryToUpdate.State = EntityState.Modified;

            Context.SaveChanges();

            OnAfterGemeindenUpdated(gemeinden);

            return gemeinden;
        }

        partial void OnGemeindenDeleted(STAVerwaltung.Models.dbSTAVerwaltung.Gemeinden item);
        partial void OnAfterGemeindenDeleted(STAVerwaltung.Models.dbSTAVerwaltung.Gemeinden item);

        public async Task<STAVerwaltung.Models.dbSTAVerwaltung.Gemeinden> DeleteGemeinden(int gemeindenr)
        {
            var itemToDelete = Context.Gemeinden
                              .Where(i => i.GemeindeNr == gemeindenr)
                              .Include(i => i.Adressen)
                              .Include(i => i.BenutzerBerechtigungGemeinden)
                              .FirstOrDefault();

            if (itemToDelete == null)
            {
               throw new Exception("Item no longer available");
            }

            OnGemeindenDeleted(itemToDelete);


            Context.Gemeinden.Remove(itemToDelete);

            try
            {
                Context.SaveChanges();
            }
            catch
            {
                Context.Entry(itemToDelete).State = EntityState.Unchanged;
                throw;
            }

            OnAfterGemeindenDeleted(itemToDelete);

            return itemToDelete;
        }
    
        public async Task ExportGemeindenArtenToExcel(Query query = null, string fileName = null)
        {
            navigationManager.NavigateTo(query != null ? query.ToUrl($"export/dbstaverwaltung/gemeindenarten/excel(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')") : $"export/dbstaverwaltung/gemeindenarten/excel(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')", true);
        }

        public async Task ExportGemeindenArtenToCSV(Query query = null, string fileName = null)
        {
            navigationManager.NavigateTo(query != null ? query.ToUrl($"export/dbstaverwaltung/gemeindenarten/csv(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')") : $"export/dbstaverwaltung/gemeindenarten/csv(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')", true);
        }

        partial void OnGemeindenArtenRead(ref IQueryable<STAVerwaltung.Models.dbSTAVerwaltung.GemeindenArten> items);

        public async Task<IQueryable<STAVerwaltung.Models.dbSTAVerwaltung.GemeindenArten>> GetGemeindenArten(Query query = null)
        {
            var items = Context.GemeindenArten.AsQueryable();


            if (query != null)
            {
                if (!string.IsNullOrEmpty(query.Expand))
                {
                    var propertiesToExpand = query.Expand.Split(',');
                    foreach(var p in propertiesToExpand)
                    {
                        items = items.Include(p.Trim());
                    }
                }

                ApplyQuery(ref items, query);
            }

            OnGemeindenArtenRead(ref items);

            return await Task.FromResult(items);
        }

        partial void OnGemeindenArtenGet(STAVerwaltung.Models.dbSTAVerwaltung.GemeindenArten item);
        partial void OnGetGemeindenArtenByGemeindeArt(ref IQueryable<STAVerwaltung.Models.dbSTAVerwaltung.GemeindenArten> items);


        public async Task<STAVerwaltung.Models.dbSTAVerwaltung.GemeindenArten> GetGemeindenArtenByGemeindeArt(string gemeindeart)
        {
            var items = Context.GemeindenArten
                              .AsNoTracking()
                              .Where(i => i.GemeindeArt == gemeindeart);

 
            OnGetGemeindenArtenByGemeindeArt(ref items);

            var itemToReturn = items.FirstOrDefault();

            OnGemeindenArtenGet(itemToReturn);

            return await Task.FromResult(itemToReturn);
        }

        partial void OnGemeindenArtenCreated(STAVerwaltung.Models.dbSTAVerwaltung.GemeindenArten item);
        partial void OnAfterGemeindenArtenCreated(STAVerwaltung.Models.dbSTAVerwaltung.GemeindenArten item);

        public async Task<STAVerwaltung.Models.dbSTAVerwaltung.GemeindenArten> CreateGemeindenArten(STAVerwaltung.Models.dbSTAVerwaltung.GemeindenArten gemeindenarten)
        {
            OnGemeindenArtenCreated(gemeindenarten);

            var existingItem = Context.GemeindenArten
                              .Where(i => i.GemeindeArt == gemeindenarten.GemeindeArt)
                              .FirstOrDefault();

            if (existingItem != null)
            {
               throw new Exception("Item already available");
            }            

            try
            {
                Context.GemeindenArten.Add(gemeindenarten);
                Context.SaveChanges();
            }
            catch
            {
                Context.Entry(gemeindenarten).State = EntityState.Detached;
                throw;
            }

            OnAfterGemeindenArtenCreated(gemeindenarten);

            return gemeindenarten;
        }

        public async Task<STAVerwaltung.Models.dbSTAVerwaltung.GemeindenArten> CancelGemeindenArtenChanges(STAVerwaltung.Models.dbSTAVerwaltung.GemeindenArten item)
        {
            var entityToCancel = Context.Entry(item);
            if (entityToCancel.State == EntityState.Modified)
            {
              entityToCancel.CurrentValues.SetValues(entityToCancel.OriginalValues);
              entityToCancel.State = EntityState.Unchanged;
            }

            return item;
        }

        partial void OnGemeindenArtenUpdated(STAVerwaltung.Models.dbSTAVerwaltung.GemeindenArten item);
        partial void OnAfterGemeindenArtenUpdated(STAVerwaltung.Models.dbSTAVerwaltung.GemeindenArten item);

        public async Task<STAVerwaltung.Models.dbSTAVerwaltung.GemeindenArten> UpdateGemeindenArten(string gemeindeart, STAVerwaltung.Models.dbSTAVerwaltung.GemeindenArten gemeindenarten)
        {
            OnGemeindenArtenUpdated(gemeindenarten);

            var itemToUpdate = Context.GemeindenArten
                              .Where(i => i.GemeindeArt == gemeindenarten.GemeindeArt)
                              .FirstOrDefault();

            if (itemToUpdate == null)
            {
               throw new Exception("Item no longer available");
            }
                
            var entryToUpdate = Context.Entry(itemToUpdate);
            entryToUpdate.CurrentValues.SetValues(gemeindenarten);
            entryToUpdate.State = EntityState.Modified;

            Context.SaveChanges();

            OnAfterGemeindenArtenUpdated(gemeindenarten);

            return gemeindenarten;
        }

        partial void OnGemeindenArtenDeleted(STAVerwaltung.Models.dbSTAVerwaltung.GemeindenArten item);
        partial void OnAfterGemeindenArtenDeleted(STAVerwaltung.Models.dbSTAVerwaltung.GemeindenArten item);

        public async Task<STAVerwaltung.Models.dbSTAVerwaltung.GemeindenArten> DeleteGemeindenArten(string gemeindeart)
        {
            var itemToDelete = Context.GemeindenArten
                              .Where(i => i.GemeindeArt == gemeindeart)
                              .Include(i => i.Gemeinden)
                              .FirstOrDefault();

            if (itemToDelete == null)
            {
               throw new Exception("Item no longer available");
            }

            OnGemeindenArtenDeleted(itemToDelete);


            Context.GemeindenArten.Remove(itemToDelete);

            try
            {
                Context.SaveChanges();
            }
            catch
            {
                Context.Entry(itemToDelete).State = EntityState.Unchanged;
                throw;
            }

            OnAfterGemeindenArtenDeleted(itemToDelete);

            return itemToDelete;
        }
    
        public async Task ExportLKZToExcel(Query query = null, string fileName = null)
        {
            navigationManager.NavigateTo(query != null ? query.ToUrl($"export/dbstaverwaltung/lkz/excel(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')") : $"export/dbstaverwaltung/lkz/excel(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')", true);
        }

        public async Task ExportLKZToCSV(Query query = null, string fileName = null)
        {
            navigationManager.NavigateTo(query != null ? query.ToUrl($"export/dbstaverwaltung/lkz/csv(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')") : $"export/dbstaverwaltung/lkz/csv(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')", true);
        }

        partial void OnLKZRead(ref IQueryable<STAVerwaltung.Models.dbSTAVerwaltung.LKZ> items);

        public async Task<IQueryable<STAVerwaltung.Models.dbSTAVerwaltung.LKZ>> GetLKZ(Query query = null)
        {
            var items = Context.LKZ.AsQueryable();


            if (query != null)
            {
                if (!string.IsNullOrEmpty(query.Expand))
                {
                    var propertiesToExpand = query.Expand.Split(',');
                    foreach(var p in propertiesToExpand)
                    {
                        items = items.Include(p.Trim());
                    }
                }

                ApplyQuery(ref items, query);
            }

            OnLKZRead(ref items);

            return await Task.FromResult(items);
        }

        partial void OnLKZGet(STAVerwaltung.Models.dbSTAVerwaltung.LKZ item);
        partial void OnGetLKZByLkz1(ref IQueryable<STAVerwaltung.Models.dbSTAVerwaltung.LKZ> items);


        public async Task<STAVerwaltung.Models.dbSTAVerwaltung.LKZ> GetLKZByLkz1(string lkz1)
        {
            var items = Context.LKZ
                              .AsNoTracking()
                              .Where(i => i.LKZ1 == lkz1);

 
            OnGetLKZByLkz1(ref items);

            var itemToReturn = items.FirstOrDefault();

            OnLKZGet(itemToReturn);

            return await Task.FromResult(itemToReturn);
        }

        partial void OnLKZCreated(STAVerwaltung.Models.dbSTAVerwaltung.LKZ item);
        partial void OnAfterLKZCreated(STAVerwaltung.Models.dbSTAVerwaltung.LKZ item);

        public async Task<STAVerwaltung.Models.dbSTAVerwaltung.LKZ> CreateLKZ(STAVerwaltung.Models.dbSTAVerwaltung.LKZ lkz)
        {
            OnLKZCreated(lkz);

            var existingItem = Context.LKZ
                              .Where(i => i.LKZ1 == lkz.LKZ1)
                              .FirstOrDefault();

            if (existingItem != null)
            {
               throw new Exception("Item already available");
            }            

            try
            {
                Context.LKZ.Add(lkz);
                Context.SaveChanges();
            }
            catch
            {
                Context.Entry(lkz).State = EntityState.Detached;
                throw;
            }

            OnAfterLKZCreated(lkz);

            return lkz;
        }

        public async Task<STAVerwaltung.Models.dbSTAVerwaltung.LKZ> CancelLKZChanges(STAVerwaltung.Models.dbSTAVerwaltung.LKZ item)
        {
            var entityToCancel = Context.Entry(item);
            if (entityToCancel.State == EntityState.Modified)
            {
              entityToCancel.CurrentValues.SetValues(entityToCancel.OriginalValues);
              entityToCancel.State = EntityState.Unchanged;
            }

            return item;
        }

        partial void OnLKZUpdated(STAVerwaltung.Models.dbSTAVerwaltung.LKZ item);
        partial void OnAfterLKZUpdated(STAVerwaltung.Models.dbSTAVerwaltung.LKZ item);

        public async Task<STAVerwaltung.Models.dbSTAVerwaltung.LKZ> UpdateLKZ(string lkz1, STAVerwaltung.Models.dbSTAVerwaltung.LKZ lkz)
        {
            OnLKZUpdated(lkz);

            var itemToUpdate = Context.LKZ
                              .Where(i => i.LKZ1 == lkz.LKZ1)
                              .FirstOrDefault();

            if (itemToUpdate == null)
            {
               throw new Exception("Item no longer available");
            }
                
            var entryToUpdate = Context.Entry(itemToUpdate);
            entryToUpdate.CurrentValues.SetValues(lkz);
            entryToUpdate.State = EntityState.Modified;

            Context.SaveChanges();

            OnAfterLKZUpdated(lkz);

            return lkz;
        }

        partial void OnLKZDeleted(STAVerwaltung.Models.dbSTAVerwaltung.LKZ item);
        partial void OnAfterLKZDeleted(STAVerwaltung.Models.dbSTAVerwaltung.LKZ item);

        public async Task<STAVerwaltung.Models.dbSTAVerwaltung.LKZ> DeleteLKZ(string lkz1)
        {
            var itemToDelete = Context.LKZ
                              .Where(i => i.LKZ1 == lkz1)
                              .Include(i => i.Adressen)
                              .Include(i => i.Bundeslaender)
                              .Include(i => i.Gemeinden)
                              .FirstOrDefault();

            if (itemToDelete == null)
            {
               throw new Exception("Item no longer available");
            }

            OnLKZDeleted(itemToDelete);


            Context.LKZ.Remove(itemToDelete);

            try
            {
                Context.SaveChanges();
            }
            catch
            {
                Context.Entry(itemToDelete).State = EntityState.Unchanged;
                throw;
            }

            OnAfterLKZDeleted(itemToDelete);

            return itemToDelete;
        }
    
        public async Task ExportLosungenToExcel(Query query = null, string fileName = null)
        {
            navigationManager.NavigateTo(query != null ? query.ToUrl($"export/dbstaverwaltung/losungen/excel(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')") : $"export/dbstaverwaltung/losungen/excel(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')", true);
        }

        public async Task ExportLosungenToCSV(Query query = null, string fileName = null)
        {
            navigationManager.NavigateTo(query != null ? query.ToUrl($"export/dbstaverwaltung/losungen/csv(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')") : $"export/dbstaverwaltung/losungen/csv(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')", true);
        }

        partial void OnLosungenRead(ref IQueryable<STAVerwaltung.Models.dbSTAVerwaltung.Losungen> items);

        public async Task<IQueryable<STAVerwaltung.Models.dbSTAVerwaltung.Losungen>> GetLosungen(Query query = null)
        {
            var items = Context.Losungen.AsQueryable();


            if (query != null)
            {
                if (!string.IsNullOrEmpty(query.Expand))
                {
                    var propertiesToExpand = query.Expand.Split(',');
                    foreach(var p in propertiesToExpand)
                    {
                        items = items.Include(p.Trim());
                    }
                }

                ApplyQuery(ref items, query);
            }

            OnLosungenRead(ref items);

            return await Task.FromResult(items);
        }

        partial void OnLosungenGet(STAVerwaltung.Models.dbSTAVerwaltung.Losungen item);
        partial void OnGetLosungenByLosungDatum(ref IQueryable<STAVerwaltung.Models.dbSTAVerwaltung.Losungen> items);


        public async Task<STAVerwaltung.Models.dbSTAVerwaltung.Losungen> GetLosungenByLosungDatum(DateTime losungdatum)
        {
            var items = Context.Losungen
                              .AsNoTracking()
                              .Where(i => i.LosungDatum == losungdatum);

 
            OnGetLosungenByLosungDatum(ref items);

            var itemToReturn = items.FirstOrDefault();

            OnLosungenGet(itemToReturn);

            return await Task.FromResult(itemToReturn);
        }

        partial void OnLosungenCreated(STAVerwaltung.Models.dbSTAVerwaltung.Losungen item);
        partial void OnAfterLosungenCreated(STAVerwaltung.Models.dbSTAVerwaltung.Losungen item);

        public async Task<STAVerwaltung.Models.dbSTAVerwaltung.Losungen> CreateLosungen(STAVerwaltung.Models.dbSTAVerwaltung.Losungen losungen)
        {
            OnLosungenCreated(losungen);

            var existingItem = Context.Losungen
                              .Where(i => i.LosungDatum == losungen.LosungDatum)
                              .FirstOrDefault();

            if (existingItem != null)
            {
               throw new Exception("Item already available");
            }            

            try
            {
                Context.Losungen.Add(losungen);
                Context.SaveChanges();
            }
            catch
            {
                Context.Entry(losungen).State = EntityState.Detached;
                throw;
            }

            OnAfterLosungenCreated(losungen);

            return losungen;
        }

        public async Task<STAVerwaltung.Models.dbSTAVerwaltung.Losungen> CancelLosungenChanges(STAVerwaltung.Models.dbSTAVerwaltung.Losungen item)
        {
            var entityToCancel = Context.Entry(item);
            if (entityToCancel.State == EntityState.Modified)
            {
              entityToCancel.CurrentValues.SetValues(entityToCancel.OriginalValues);
              entityToCancel.State = EntityState.Unchanged;
            }

            return item;
        }

        partial void OnLosungenUpdated(STAVerwaltung.Models.dbSTAVerwaltung.Losungen item);
        partial void OnAfterLosungenUpdated(STAVerwaltung.Models.dbSTAVerwaltung.Losungen item);

        public async Task<STAVerwaltung.Models.dbSTAVerwaltung.Losungen> UpdateLosungen(DateTime losungdatum, STAVerwaltung.Models.dbSTAVerwaltung.Losungen losungen)
        {
            OnLosungenUpdated(losungen);

            var itemToUpdate = Context.Losungen
                              .Where(i => i.LosungDatum == losungen.LosungDatum)
                              .FirstOrDefault();

            if (itemToUpdate == null)
            {
               throw new Exception("Item no longer available");
            }
                
            var entryToUpdate = Context.Entry(itemToUpdate);
            entryToUpdate.CurrentValues.SetValues(losungen);
            entryToUpdate.State = EntityState.Modified;

            Context.SaveChanges();

            OnAfterLosungenUpdated(losungen);

            return losungen;
        }

        partial void OnLosungenDeleted(STAVerwaltung.Models.dbSTAVerwaltung.Losungen item);
        partial void OnAfterLosungenDeleted(STAVerwaltung.Models.dbSTAVerwaltung.Losungen item);

        public async Task<STAVerwaltung.Models.dbSTAVerwaltung.Losungen> DeleteLosungen(DateTime losungdatum)
        {
            var itemToDelete = Context.Losungen
                              .Where(i => i.LosungDatum == losungdatum)
                              .FirstOrDefault();

            if (itemToDelete == null)
            {
               throw new Exception("Item no longer available");
            }

            OnLosungenDeleted(itemToDelete);


            Context.Losungen.Remove(itemToDelete);

            try
            {
                Context.SaveChanges();
            }
            catch
            {
                Context.Entry(itemToDelete).State = EntityState.Unchanged;
                throw;
            }

            OnAfterLosungenDeleted(itemToDelete);

            return itemToDelete;
        }
    
        public async Task ExportOrganisationenToExcel(Query query = null, string fileName = null)
        {
            navigationManager.NavigateTo(query != null ? query.ToUrl($"export/dbstaverwaltung/organisationen/excel(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')") : $"export/dbstaverwaltung/organisationen/excel(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')", true);
        }

        public async Task ExportOrganisationenToCSV(Query query = null, string fileName = null)
        {
            navigationManager.NavigateTo(query != null ? query.ToUrl($"export/dbstaverwaltung/organisationen/csv(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')") : $"export/dbstaverwaltung/organisationen/csv(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')", true);
        }

        partial void OnOrganisationenRead(ref IQueryable<STAVerwaltung.Models.dbSTAVerwaltung.Organisationen> items);

        public async Task<IQueryable<STAVerwaltung.Models.dbSTAVerwaltung.Organisationen>> GetOrganisationen(Query query = null)
        {
            var items = Context.Organisationen.AsQueryable();


            if (query != null)
            {
                if (!string.IsNullOrEmpty(query.Expand))
                {
                    var propertiesToExpand = query.Expand.Split(',');
                    foreach(var p in propertiesToExpand)
                    {
                        items = items.Include(p.Trim());
                    }
                }

                ApplyQuery(ref items, query);
            }

            OnOrganisationenRead(ref items);

            return await Task.FromResult(items);
        }

        partial void OnOrganisationenGet(STAVerwaltung.Models.dbSTAVerwaltung.Organisationen item);
        partial void OnGetOrganisationenByOrganisationCode(ref IQueryable<STAVerwaltung.Models.dbSTAVerwaltung.Organisationen> items);


        public async Task<STAVerwaltung.Models.dbSTAVerwaltung.Organisationen> GetOrganisationenByOrganisationCode(string organisationcode)
        {
            var items = Context.Organisationen
                              .AsNoTracking()
                              .Where(i => i.OrganisationCode == organisationcode);

 
            OnGetOrganisationenByOrganisationCode(ref items);

            var itemToReturn = items.FirstOrDefault();

            OnOrganisationenGet(itemToReturn);

            return await Task.FromResult(itemToReturn);
        }

        partial void OnOrganisationenCreated(STAVerwaltung.Models.dbSTAVerwaltung.Organisationen item);
        partial void OnAfterOrganisationenCreated(STAVerwaltung.Models.dbSTAVerwaltung.Organisationen item);

        public async Task<STAVerwaltung.Models.dbSTAVerwaltung.Organisationen> CreateOrganisationen(STAVerwaltung.Models.dbSTAVerwaltung.Organisationen organisationen)
        {
            OnOrganisationenCreated(organisationen);

            var existingItem = Context.Organisationen
                              .Where(i => i.OrganisationCode == organisationen.OrganisationCode)
                              .FirstOrDefault();

            if (existingItem != null)
            {
               throw new Exception("Item already available");
            }            

            try
            {
                Context.Organisationen.Add(organisationen);
                Context.SaveChanges();
            }
            catch
            {
                Context.Entry(organisationen).State = EntityState.Detached;
                throw;
            }

            OnAfterOrganisationenCreated(organisationen);

            return organisationen;
        }

        public async Task<STAVerwaltung.Models.dbSTAVerwaltung.Organisationen> CancelOrganisationenChanges(STAVerwaltung.Models.dbSTAVerwaltung.Organisationen item)
        {
            var entityToCancel = Context.Entry(item);
            if (entityToCancel.State == EntityState.Modified)
            {
              entityToCancel.CurrentValues.SetValues(entityToCancel.OriginalValues);
              entityToCancel.State = EntityState.Unchanged;
            }

            return item;
        }

        partial void OnOrganisationenUpdated(STAVerwaltung.Models.dbSTAVerwaltung.Organisationen item);
        partial void OnAfterOrganisationenUpdated(STAVerwaltung.Models.dbSTAVerwaltung.Organisationen item);

        public async Task<STAVerwaltung.Models.dbSTAVerwaltung.Organisationen> UpdateOrganisationen(string organisationcode, STAVerwaltung.Models.dbSTAVerwaltung.Organisationen organisationen)
        {
            OnOrganisationenUpdated(organisationen);

            var itemToUpdate = Context.Organisationen
                              .Where(i => i.OrganisationCode == organisationen.OrganisationCode)
                              .FirstOrDefault();

            if (itemToUpdate == null)
            {
               throw new Exception("Item no longer available");
            }
                
            var entryToUpdate = Context.Entry(itemToUpdate);
            entryToUpdate.CurrentValues.SetValues(organisationen);
            entryToUpdate.State = EntityState.Modified;

            Context.SaveChanges();

            OnAfterOrganisationenUpdated(organisationen);

            return organisationen;
        }

        partial void OnOrganisationenDeleted(STAVerwaltung.Models.dbSTAVerwaltung.Organisationen item);
        partial void OnAfterOrganisationenDeleted(STAVerwaltung.Models.dbSTAVerwaltung.Organisationen item);

        public async Task<STAVerwaltung.Models.dbSTAVerwaltung.Organisationen> DeleteOrganisationen(string organisationcode)
        {
            var itemToDelete = Context.Organisationen
                              .Where(i => i.OrganisationCode == organisationcode)
                              .Include(i => i.BenutzerBerechtigungOrganisationen)
                              .Include(i => i.Gemeinden)
                              .FirstOrDefault();

            if (itemToDelete == null)
            {
               throw new Exception("Item no longer available");
            }

            OnOrganisationenDeleted(itemToDelete);


            Context.Organisationen.Remove(itemToDelete);

            try
            {
                Context.SaveChanges();
            }
            catch
            {
                Context.Entry(itemToDelete).State = EntityState.Unchanged;
                throw;
            }

            OnAfterOrganisationenDeleted(itemToDelete);

            return itemToDelete;
        }
    
        public async Task ExportReligionenToExcel(Query query = null, string fileName = null)
        {
            navigationManager.NavigateTo(query != null ? query.ToUrl($"export/dbstaverwaltung/religionen/excel(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')") : $"export/dbstaverwaltung/religionen/excel(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')", true);
        }

        public async Task ExportReligionenToCSV(Query query = null, string fileName = null)
        {
            navigationManager.NavigateTo(query != null ? query.ToUrl($"export/dbstaverwaltung/religionen/csv(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')") : $"export/dbstaverwaltung/religionen/csv(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')", true);
        }

        partial void OnReligionenRead(ref IQueryable<STAVerwaltung.Models.dbSTAVerwaltung.Religionen> items);

        public async Task<IQueryable<STAVerwaltung.Models.dbSTAVerwaltung.Religionen>> GetReligionen(Query query = null)
        {
            var items = Context.Religionen.AsQueryable();


            if (query != null)
            {
                if (!string.IsNullOrEmpty(query.Expand))
                {
                    var propertiesToExpand = query.Expand.Split(',');
                    foreach(var p in propertiesToExpand)
                    {
                        items = items.Include(p.Trim());
                    }
                }

                ApplyQuery(ref items, query);
            }

            OnReligionenRead(ref items);

            return await Task.FromResult(items);
        }

        partial void OnReligionenGet(STAVerwaltung.Models.dbSTAVerwaltung.Religionen item);
        partial void OnGetReligionenByReligionCode(ref IQueryable<STAVerwaltung.Models.dbSTAVerwaltung.Religionen> items);


        public async Task<STAVerwaltung.Models.dbSTAVerwaltung.Religionen> GetReligionenByReligionCode(string religioncode)
        {
            var items = Context.Religionen
                              .AsNoTracking()
                              .Where(i => i.ReligionCode == religioncode);

 
            OnGetReligionenByReligionCode(ref items);

            var itemToReturn = items.FirstOrDefault();

            OnReligionenGet(itemToReturn);

            return await Task.FromResult(itemToReturn);
        }

        partial void OnReligionenCreated(STAVerwaltung.Models.dbSTAVerwaltung.Religionen item);
        partial void OnAfterReligionenCreated(STAVerwaltung.Models.dbSTAVerwaltung.Religionen item);

        public async Task<STAVerwaltung.Models.dbSTAVerwaltung.Religionen> CreateReligionen(STAVerwaltung.Models.dbSTAVerwaltung.Religionen religionen)
        {
            OnReligionenCreated(religionen);

            var existingItem = Context.Religionen
                              .Where(i => i.ReligionCode == religionen.ReligionCode)
                              .FirstOrDefault();

            if (existingItem != null)
            {
               throw new Exception("Item already available");
            }            

            try
            {
                Context.Religionen.Add(religionen);
                Context.SaveChanges();
            }
            catch
            {
                Context.Entry(religionen).State = EntityState.Detached;
                throw;
            }

            OnAfterReligionenCreated(religionen);

            return religionen;
        }

        public async Task<STAVerwaltung.Models.dbSTAVerwaltung.Religionen> CancelReligionenChanges(STAVerwaltung.Models.dbSTAVerwaltung.Religionen item)
        {
            var entityToCancel = Context.Entry(item);
            if (entityToCancel.State == EntityState.Modified)
            {
              entityToCancel.CurrentValues.SetValues(entityToCancel.OriginalValues);
              entityToCancel.State = EntityState.Unchanged;
            }

            return item;
        }

        partial void OnReligionenUpdated(STAVerwaltung.Models.dbSTAVerwaltung.Religionen item);
        partial void OnAfterReligionenUpdated(STAVerwaltung.Models.dbSTAVerwaltung.Religionen item);

        public async Task<STAVerwaltung.Models.dbSTAVerwaltung.Religionen> UpdateReligionen(string religioncode, STAVerwaltung.Models.dbSTAVerwaltung.Religionen religionen)
        {
            OnReligionenUpdated(religionen);

            var itemToUpdate = Context.Religionen
                              .Where(i => i.ReligionCode == religionen.ReligionCode)
                              .FirstOrDefault();

            if (itemToUpdate == null)
            {
               throw new Exception("Item no longer available");
            }
                
            var entryToUpdate = Context.Entry(itemToUpdate);
            entryToUpdate.CurrentValues.SetValues(religionen);
            entryToUpdate.State = EntityState.Modified;

            Context.SaveChanges();

            OnAfterReligionenUpdated(religionen);

            return religionen;
        }

        partial void OnReligionenDeleted(STAVerwaltung.Models.dbSTAVerwaltung.Religionen item);
        partial void OnAfterReligionenDeleted(STAVerwaltung.Models.dbSTAVerwaltung.Religionen item);

        public async Task<STAVerwaltung.Models.dbSTAVerwaltung.Religionen> DeleteReligionen(string religioncode)
        {
            var itemToDelete = Context.Religionen
                              .Where(i => i.ReligionCode == religioncode)
                              .Include(i => i.AdressenEreignisse)
                              .FirstOrDefault();

            if (itemToDelete == null)
            {
               throw new Exception("Item no longer available");
            }

            OnReligionenDeleted(itemToDelete);


            Context.Religionen.Remove(itemToDelete);

            try
            {
                Context.SaveChanges();
            }
            catch
            {
                Context.Entry(itemToDelete).State = EntityState.Unchanged;
                throw;
            }

            OnAfterReligionenDeleted(itemToDelete);

            return itemToDelete;
        }
        }
}