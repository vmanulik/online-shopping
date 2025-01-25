using System.ComponentModel;
using OnlineShopping.CatalogService.Application.Common.Interfaces;
using OnlineShopping.Shared.Domain.Entities;

namespace OnlineShopping.CatalogService.Application.Common.Services;

public class DictionarizeService<T> : IDictionarizeService<T> where T : BaseEntity
{
    public IDictionary<string, string> CreateDictionary(T entity)
    {
        if (entity == null) { throw new NullReferenceException(); }

        var dictionary = new Dictionary<string, string>();
        foreach (PropertyDescriptor property in TypeDescriptor.GetProperties(entity))
        {
            object value = property.GetValue(entity)!;
            dictionary.Add(property.Name, value?.ToString()!);
            
        }

        return dictionary;
    }
}
