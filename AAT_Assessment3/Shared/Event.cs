using System;
using System.ComponentModel.DataAnnotations;

namespace AAT_Assessment3.Shared
{
    public class Event
    {
        public int EventId { get; set; }

        [Required(ErrorMessage = "Name is required")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Date is required")]
        [DataType(DataType.Date)]
        public DateTime Date { get; set; }

        [Required(ErrorMessage = "TotalSeats is required")]
        [Range(1, int.MaxValue, ErrorMessage = "TotalSeats must be at least 1")]
        public int TotalSeats { get; set; }

        [Required(ErrorMessage = "AvailableSeats is required")]
        [Range(0, int.MaxValue, ErrorMessage = "AvailableSeats cannot be negative")]
        public int AvailableSeats { get; set; }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (AvailableSeats > TotalSeats)
            {
                yield return new ValidationResult(
                    "AvailableSeats cannot be greater than TotalSeats.",
                    new[] { nameof(AvailableSeats) }
                );
            }
        }
    }
}
