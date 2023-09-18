namespace BusinessCard.Application.Application.Common.Models;

public static class PropertyHelper
{
    public static string GetPropertySorterName(string sorter, SortOrderEnum sortOrder = SortOrderEnum.ASC)
    {
        var sortByString = $"p.Id {sortOrder}";

        if (!string.IsNullOrEmpty(sorter))
            switch (sorter)
            {
                case "property_id":
                    sortByString = $"p.Id {sortOrder}";
                    break;
                case "property_name":
                    sortByString = $"p.Name {sortOrder}";
                    break;
                case "land_area":
                    sortByString = $"p.LandArea {sortOrder}";
                    break;
                case "towers":
                    sortByString = $"COUNT(t.Id) {sortOrder}";
                    break;
                default:
                    sortByString = "p.Id ASC";
                    break;
            }

        return sortByString;
    }

    public static string GetTowerSorterName(string sorter, SortOrderEnum sortOrder = SortOrderEnum.ASC)
    {
        var sortByString = $"t.Id {sortOrder}";

        if (!string.IsNullOrEmpty(sorter))
            switch (sorter)
            {
                case "tower_id":
                    sortByString = $"t.Id {sortOrder}";
                    break;
                case "property_id":
                    sortByString = $"p.Id {sortOrder}";
                    break;
                case "property_name":
                    sortByString = $"p.[Name] {sortOrder}";
                    break;
                case "tower_name":
                    sortByString = $"t.[Name] {sortOrder}";
                    break;
                case "units":
                    sortByString = $"COUNT(u.[Id]) {sortOrder}";
                    break;
                default:
                    sortByString = "t.Id ASC";
                    break;
            }

        return sortByString;
    }


    public static string GetFloorSorterName(string sorter, SortOrderEnum sortOrder = SortOrderEnum.ASC)
    {
        var sortByString = $"f.Id {sortOrder}";

        if (!string.IsNullOrEmpty(sorter))
            switch (sorter)
            {
                case "floor_id":
                    sortByString = $"f.Id {sortOrder}";
                    break;
                case "tower_id":
                    sortByString = $"t.Id {sortOrder}";
                    break;
                case "property_id":
                    sortByString = $"p.Id {sortOrder}";
                    break;
                case "property_name":
                    sortByString = $"p.[Name] {sortOrder}";
                    break;
                case "tower_name":
                    sortByString = $"t.[Name] {sortOrder}";
                    break;
                case "floor_name":
                    sortByString = $"f.[Name] {sortOrder}";
                    break;
                case "units":
                    sortByString = $"COUNT(u.[Id]) {sortOrder}";
                    break;
                default:
                    sortByString = "f.Id ASC";
                    break;
            }

        return sortByString;
    }


    public static string GetUnitSorterName(string sorter, SortOrderEnum sortOrder = SortOrderEnum.ASC)
    {
        var sortByString = $"u.Id {sortOrder}";

        if (!string.IsNullOrEmpty(sorter))
            switch (sorter)
            {
                case "floor_id":
                    sortByString = $"f.Id {sortOrder}";
                    break;
                case "tower_id":
                    sortByString = $"t.Id {sortOrder}";
                    break;
                case "property_id":
                    sortByString = $"p.Id {sortOrder}";
                    break;
                case "property_name":
                    sortByString = $"p.[Name] {sortOrder}";
                    break;
                case "tower_name":
                    sortByString = $"t.[Name] {sortOrder}";
                    break;
                case "floor_name":
                    sortByString = $"f.[Name] {sortOrder}";
                    break;
                case "unit_type":
                    sortByString = $"ut.[ShortCode] {sortOrder}";
                    break;
                case "scenic_view":
                    sortByString = $"sv.[Name] {sortOrder}";
                    break;
                case "total_area":
                    sortByString = $"u.FloorArea+u.BalconyArea {sortOrder}";
                    break;
                case "floor_area":
                    sortByString = $"u.FloorArea {sortOrder}";
                    break;
                case "balcony_area":
                    sortByString = $"u.BalconyArea {sortOrder}";
                    break;
                case "identifier":
                    sortByString = $"u.Identifier {sortOrder}";
                    break;
                case "price":
                    sortByString = $"u.Price {sortOrder}";
                    break;
                case "unit_status":
                    sortByString = $"us.Name {sortOrder}";
                    break;
                default:
                    sortByString = "f.Id ASC";
                    break;
            }

        return sortByString;
    }
}