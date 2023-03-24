using Building.DAL.Interfaces;
using Building.Domain.Entity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Building.DAL.Repositories
{
    public class MaterialRepository : IMaterialRepository
    {
        private readonly BuildingContext buildingContext;
        public MaterialRepository(BuildingContext buildingContext)
        {
            this.buildingContext = buildingContext;
        }
        public bool Create(Material entity)
        {
            throw new NotImplementedException();
        }

        public bool Delete(int id)
        {
            throw new NotImplementedException();
        }

        public async Task<Material> Get(int id)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<Catalog> GetCatalogs()
        {
            return buildingContext.Catalogs.ToList();
        }



        /// <summary>
        /// get material if material name contains term
        /// </summary>
        /// <param name="term"></param>
        /// <returns></returns>
        public IEnumerable<string> GetMaterial(string term)
        {
            return buildingContext.Materials.Where(c=>c.Name.Contains(term)).Select(c=>c.Name).ToList();
        }

        public IEnumerable<Material> GetMaterials(int id)
        {
            return buildingContext.Materials.Where(c=>c.IdCatalog==id).ToList();
        }

        //public IEnumerable<MaterialCharacteristic> GetMaterialCharacteristic(string materialName)
        //{
        //    return buildingContext.MaterialCharacteristics
        //        .Include(c=>c.Material).ThenInclude(c=>c.Catalog)
        //        .Include(c=>c.Charac)
        //        .Where(c=>c.Material.Name==materialName);

        //}

        public IEnumerable<string> GetMaterialName()
        {
            return   buildingContext.Materials.Select(x=>x.Name);
        }

        public Task<bool> Update(Material entity)
        {
            throw new NotImplementedException();
        }

        public async Task<Material> GetIdByName(string name)
        {
            return await buildingContext.Materials.FirstOrDefaultAsync(c => c.Name == name);
        }
    }
}
