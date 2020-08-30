using Microsoft.AspNetCore.Mvc.Rendering;

namespace CoffeeHouse.Services.CustomSelectList.Interfaces
{
    public interface ICustomSelectList
    {
        SelectList CreateListOfCategoryNames();
        SelectList CreateListOfCategoryNames(int defaultItemId);
        SelectList CreateListOfClientNames();
        SelectList CreateListOfClientNames(string defaultItemId);
        SelectList CreateListOfCashierFullNames();
        SelectList CreateListOfCashierFullNames(int defaultItemId);
        SelectList CreateListOfProductFullNames();
        SelectList CreateListOfProductFullNames(int defaultItemId);
        SelectList CreateListOfOrderProdMarks();
        SelectList CreateListOfOrderProdMarks(string defaultMark);
    }
}
