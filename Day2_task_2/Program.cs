using System;
using System.Linq;

namespace Day2_task_2
{
/*
    OCP. Методы платежей

    Сейчас система не готова к расширению. К сожалению при добавление нового
    способа оплаты, нам нужно модифицировать все ифы которые совершают те или
    иные действия с разными системами.
    Необходимо зафиксировать интерфейс платежёной системы и сокрыть их
    многообразие под какой-нибудь сущностью. Например фабрикой (или фабричным
    методом).
    Важное условие: пользователь вводит именно идентификатор платёжной системы.
*/
    class Program
    {
        static void Main(string[] args)
        {

            var orderForm = new OrderForm();
            PaymentHandler paymentHandler = new PaymentHandler(new QIWIPayment(), new WebMoneyPayment(), new CardPayment());

            var systemId = orderForm.ShowForm(paymentHandler);

            Payment payment = paymentHandler.GetPayment(systemId);

            payment.StartPaymentProcess();
            payment.ShowPaymentResult();
        }
    }

    public abstract class Payment
    {
        public readonly string Name;

        protected Payment(string name)
        {
            Name = name;
        }

        public void ShowPaymentResult()
        {
            Console.WriteLine($"Вы оплатили с помощью {Name}");
            ShowPaymentDetails();
            Console.WriteLine("Оплата прошла успешно!");
        }
        public abstract void StartPaymentProcess();
        protected abstract void ShowPaymentDetails();
    }
    public class QIWIPayment : Payment
    {
        public QIWIPayment() : base("QIWI") { }

        protected override void ShowPaymentDetails()
        {
            Console.WriteLine("Проверка платежа через QIWI...");
        }

        public override void StartPaymentProcess()
        {
            Console.WriteLine("Перевод на страницу QIWI...");
        }
    }
    public class WebMoneyPayment : Payment
    {
        public WebMoneyPayment() : base("WebMoney") { }

        protected override void ShowPaymentDetails()
        {
            Console.WriteLine("Проверка платежа через WebMoney...");
        }

        public override void StartPaymentProcess()
        {
            Console.WriteLine("Вызов API WebMoney...");
        }
    }
    public class CardPayment : Payment
    {
        public CardPayment() : base("Card") { }

        protected override void ShowPaymentDetails()
        {
            Console.WriteLine("Проверка платежа через Card...");
        }

        public override void StartPaymentProcess()
        {
            Console.WriteLine("Вызов API банка эмитера карты Card...");
        }
    }

    public class PaymentHandler
    {
        Payment[] _payments;

        public PaymentHandler(params Payment[] payments)
        {
            _payments = payments;
        }

        public Payment GetPayment(string name)
        {
            return _payments.First(p => p.Name == name);
        }

        public string GetPaymentMethods()
        {
            return string.Join(",", _payments.Select(p => p.Name));
        }
    }

    public class OrderForm
    {
        public string ShowForm(PaymentHandler paymentHandler)
        {
            Console.WriteLine("Мы принимаем: " + paymentHandler.GetPaymentMethods());

            //симуляция веб интерфейса
            Console.WriteLine("Какое системой вы хотите совершить оплату?");
            return Console.ReadLine();
        }
    }
}

