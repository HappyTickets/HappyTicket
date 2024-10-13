using Domain.Entities.ContactEntity;

namespace Application.Interfaces.IContactService
{
    public interface IContactService
    {
        Task SendMessageAsync(Contact contact, CancellationToken cancellationToken);
    }

}
