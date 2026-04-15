using CourseWork.DTOs;
using CourseWork.Models;

namespace CourseWork.Mappers;

public static class FoodTypeMappers
{
    public static FoodType ToEntity(this FoodTypeDto foodTypeDto)
    {
        return new FoodType()
        {
            Brand = foodTypeDto.Brand,
            MinThreshold = foodTypeDto.MinThreshold,
            Name = foodTypeDto.Name,
            Unit = foodTypeDto.Unit,
            StockQuantity = foodTypeDto.StockQuantity
        };
    }

    public static FoodTypeDto ToDto(this FoodType foodType)
    {
        return new FoodTypeDto()
        {
            Id = foodType.Id,
            Brand = foodType.Brand,
            MinThreshold = foodType.MinThreshold,
            Name = foodType.Name,
            Unit = foodType.Unit,
            StockQuantity = foodType.StockQuantity
        };
    }
}