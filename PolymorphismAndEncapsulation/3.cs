using System;

public interface IOrderState
{
    void Process(Order order);
    void Cancel(Order order);
    void Complete(Order order);
}

public class NewOrderState : IOrderState
{
    public void Cancel(Order order)
    {
        Console.WriteLine("Заказ отменен");
        order.ChangeState(new CanceledState());
    }

    public void Process(Order order)
    {
        Console.WriteLine("Обработка заказа");
        order.ChangeState(new ProcessingState());
    }

    public void Complete(Order order)
    {
        Console.WriteLine("Заказ завершён");
        order.ChangeState(new CompletedState());
    }
}

public class ProcessingState : IOrderState
{
    public void Cancel(Order order)
    {
        Console.WriteLine("Заказ отменен");
        order.ChangeState(new CanceledState());
    }

    public void Process(Order order)
    {
        Console.WriteLine("Обработка заказа");
        order.ChangeState(new ShippedState());
    }

    public void Complete(Order order)
    {
        Console.WriteLine("Заказ завершён");
        order.ChangeState(new CompletedState());
    }
}

public class ShippedState : IOrderState
{
    public void Cancel(Order order)
    {
        Console.WriteLine("Заказ отменен");
        order.ChangeState(new CanceledState());
    }

    public void Process(Order order)
    {
        Console.WriteLine("Обработка заказа");
        order.ChangeState(new CompletedState());
    }

    public void Complete(Order order)
    {
        Console.WriteLine("Заказ завершён");
        order.ChangeState(new CompletedState());
    }
}

public class CompletedState : IOrderState
{
    public void Cancel(Order order)
    {
        throw new Exception("Нельзя отменить завершённый заказ");
    }

    public void Process(Order order)
    {
        throw new Exception("Нельзя обработать завершённый заказ");
    }

    public void Complete(Order order)
    {
        throw new Exception("Нельзя завершить завершённый заказ");
    }
}

public class CanceledState : IOrderState
{
    public void Cancel(Order order)
    {
        Console.WriteLine("Заказ уже отменён");
    }

    public void Process(Order order)
    {
        throw new Exception("Нельзя обработать отменённый заказ");
    }

    public void Complete(Order order)
    {
        throw new Exception("Нельзя завершить отменённый заказ");
    }
}

public class Order
{
    public event Action<string> StateChanged;
    private void OnStateChanged()
        => StateChanged?.Invoke($"Новое состояние: {_state.GetType().Name}");

    private IOrderState _state = new NewOrderState();

    public void ChangeState(IOrderState newState)
    {
        _state = newState;
        OnStateChanged();
    }

    public void Process() => _state.Process(this);
    public void Cancel() => _state.Cancel(this);
    public void Complete() => _state.Complete(this);
}

public class Program
{
    static void Main()
    {
        var order = new Order();
        order.StateChanged += Console.WriteLine;

        order.Process();
        order.Cancel();
        order.Process();

        var order2 = new Order();
        order2.StateChanged += Console.WriteLine;

        order2.Process();
        order2.Process();
        order2.Complete();
        order2.Cancel();
    }

}