namespace Product.Application.DTO
{
    public class CategoryDTO
    {
        public string Name { get; set; }
    }

    public class UpdateCategoryDTO
    {
        public long Id { get; set; }
        public string Name { get; set; }
    }

    public class CategoryResponseDTO
    {
        public string Name { get; set; }
        public string CreatedDate { get; set; }
    }
}
