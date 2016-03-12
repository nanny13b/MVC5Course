using System;
using System.Linq;
using System.Collections.Generic;

namespace MVC5Course.Models
{
    public class ProductRepository : EFRepository<Product>, IProductRepository
    {


        //over + " " ��n�Ƽg��function + enter
        //==>

        public override IQueryable<Product> All()
        {
            return base.All().Where(p => p.IsDeleted == false);

            //Will Sample Code
            //return base.All().Where(p => !p.IsDeleted); 
        }

        public IQueryable<Product> All(bool isAll)
        {
            //Will Sample Code: �`�N base �M this ���t�O
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
            //Will Sample Code: ���� �g�b FirstOrDefault �̭� ���Φh�gWhere ����
            return this.All().FirstOrDefault(p => p.ProductId == id);
            // Cindy : return this.All().Where(p => p.ProductId == id.Value).FirstOrDefault();
            //throw new NotImplementedException();
        }
    }

    public interface IProductRepository : IRepository<Product>
    {

    }
}