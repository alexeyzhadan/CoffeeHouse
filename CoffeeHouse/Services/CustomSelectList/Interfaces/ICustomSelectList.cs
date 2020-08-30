using Microsoft.AspNetCore.Mvc.Rendering;

namespace CoffeeHouse.Services.CustomSelectList.Interfaces
{
    public interface ICustomSelectList
    {
        SelectList CreateListOfCategoryNames();
        SelectList CreateListOfCategoryNames(int defaultIntId);
        SelectList CreateListOfClientNames();
        SelectList CreateListOfClientNames(int defaultIntId);
        SelectList CreateListOfCashierFullNames();
        SelectList CreateListOfCashierFullNames(int defaultIntId);
        SelectList CreateListOfProductFullNames();
        SelectList CreateListOfProductFullNames(int defaultIntId);
        SelectList CreateListOfOrderProdMarks();
        SelectList CreateListOfOrderProdMarks(string defaultMark);
    }
}
