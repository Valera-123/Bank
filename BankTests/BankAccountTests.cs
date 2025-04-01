using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using BankAccountNS;

namespace BankTests
{
    [TestClass]
    public class BankAccountTests
    {
        [TestMethod]
        public void Debit_WithValidAmount_UpdatesBalance()
        {
            // Arrange
            double beginningBalance = 11.99;
            double debitAmount = 4.55;
            double expected = 7.44;
            BankAccount account = new BankAccount("Mr. Bryan Walton", beginningBalance);

            // Act
            account.Debit(debitAmount);

            // Assert
            double actual = account.Balance;
            Assert.AreEqual(expected, actual, 0.001, "Account not debited correctly");
        }

        [TestMethod]
        public void Debit_WhenAmountIsLessThanZero_ShouldThrowArgumentOutOfRange()
        {
            // Arrange
            double beginningBalance = 11.99;
            double debitAmount = -100.00;
            BankAccount account = new BankAccount("Mr. Bryan Walton", beginningBalance);

            // Act and assert
            Assert.ThrowsException<System.ArgumentOutOfRangeException>(() => account.Debit(debitAmount));
        }

        [TestMethod]
        public void Debit_WhenAmountIsMoreThanBalance_ShouldThrowArgumentOutOfRange()
        {
            // Arrange
            double beginningBalance = 11.99;
            double debitAmount = 20.0;
            BankAccount account = new BankAccount("Mr. Bryan Walton", beginningBalance);

            // Act
            try
            {
                account.Debit(debitAmount);
            }
            catch (System.ArgumentOutOfRangeException e)
            {
                // Assert
                StringAssert.Contains(e.Message, BankAccount.DebitAmountExceedsBalanceMessage);
                return;
            }

            Assert.Fail("The expected exception was not thrown.");
        }

        //проверка на пустую строку в имени
        [TestMethod]
        public void AccountCreation_WithEmptyName_ShouldThrow()
        {
            // Arrange
            // Act
            // Assert
            var ex = Assert.ThrowsException<ArgumentException>(() => {
                var account = new BankAccount("", 1000); // Пустое имя!
            });

            Assert.IsTrue(ex.Message.Contains("Имя не может быть пустым"),
                "Банк должен требовать имя!");
        }

        //проверка на миллиардера!
        [TestMethod]
        public void Debit_WithCrazyRichBalance_ShouldWork()
        {
            // Arrange
            // Дано: у нас триллион рублей (и 50 руб. на лимонад)
            var account = new BankAccount("Олег Тинькофф", 1_000_000_000_050.00);

            // Act
            // Когда: снимаем 50 рублей на пачку чипсов
            account.Debit(50.00);

            // Assert
            // Тогда: должен остаться триллион (и 0 на лимонад)
            Assert.AreEqual(1_000_000_000_000.00, account.Balance,
                "Даже у миллиардеров баланс должен уменьшаться!");
        }


    }
}
