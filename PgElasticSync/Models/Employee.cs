using System.ComponentModel.DataAnnotations.Schema;
using Nest;

namespace PgElasticSync.Models;

public class Employee
{
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string? MiddleName { get; set; }
    public string Email { get; set; }
    public string PhoneNumber { get; set; }
    public string City { get; set; }
    public DateTime DateOfBirth { get; set; }
    [Keyword]
    public string University { get; set; }
    [Column(TypeName = "varchar(24)")]
    public DegreeOfEducation DegreeOfEducation { get; set; }
    public DateTime StartWorkingDate { get; set; }
    public DateTime? EndWorkingDate { get; set; }
    public string PositionName { get; set; }
}

public enum DegreeOfEducation
{
    Bachelor = 0,
    Master = 1
}