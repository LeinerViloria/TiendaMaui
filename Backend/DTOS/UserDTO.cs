namespace Backend.DTOS;

public record UserDTO
{
    public int Rowid {get; set;}
    public string Email {get; set;} = null!;
}