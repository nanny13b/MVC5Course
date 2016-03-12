using System;
using System.Linq;
using System.Collections.Generic;

namespace MVC5Course.Models
{
    public class ProductRepository : EFRepository<Product>, IProductRepository
    {


        //over + " " 選要複寫的function + enter
        //==>

        public override IQueryable<Product> All()
        {
            return base.All().Where(p => p.IsDeleted == false);

            //Will Sample Code
            //return base.All().Where(p => !p.IsDeleted); 
        }

        public IQueryable<Product> All(bool isAll)
        {
            //Will Sample Code: 注意 base 和 this 的差別
            if (isAll)
            {
                return base.All();
            }
            else
            {
                return this.All();
            }
        }

        public Product Find(int? id)
        {
            //Will Sample Code: 條件式 寫在 FirstOrDefault 裡面 不用多寫Where 條件式
            return this.All().FirstOrDefault(p => p.ProductId == id);
            // Cindy : return this.All().Where(p => p.ProductId == id.Value).FirstOrDefault();
            //throw new NotImplementedException();
        }
    }

    public interface IProductRepository : IRepository<Product>
    {

    }
}