using System;
using System.ComponentModel.DataAnnotations;

namespace CIE206PROJECT.Models
{
public class salary_pt
{
	public int user_id { get; set; }

	public int hours_worked { get; set; }

	public int pay_per_session { get; set; }

	public int pay_in_month { get; set; }

}
}