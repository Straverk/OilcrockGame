namespace Player.Modules.Interactible
{
    public interface IInteractable
    {
        public string InteractName { get; }

        public void Interact();
    }

    public interface ITakeable : IInteractable
    {
        public virtual void PrimaryAction() { }
        public virtual void SecondaryAction() { }
    }
}