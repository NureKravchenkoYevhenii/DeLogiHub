using System.ComponentModel.DataAnnotations;

namespace Domain.Models;
public abstract class BaseEntity
{
	[Required]
	public virtual Guid Id { get; set; }
}
