# Project 19 : Phonebook WPF

<img align="right" width="100" height="100" src="https://github.com/rozhkovsvyat/Project19.WPF/assets/71471748/530dedd2-4fa9-4b5f-aaf3-61bce8b6b315">
<img align="right" width="100" height="100" src="https://github.com/rozhkovsvyat/Project19.WPF/assets/71471748/f40f2155-4c9a-4f8c-9754-4e10d46bd57c">

**#net7.0.10-windows**


Wpf-клиент проекта Phonebook на базе [API](https://github.com/rozhkovsvyat/Project19.API/) + установщик

Аналог [Web-клиента](https://github.com/rozhkovsvyat/Project19.Web/), собранный на архитектуре MVVM

> :link: [Использует общие библиотеки](https://github.com/rozhkovsvyat/Project19.Libs) ([+ фабрика](https://github.com/rozhkovsvyat/Tools.RecipeFactory) [+ инструменты](https://github.com/rozhkovsvyat/Tools.WPF))
> 
> :link: [Использует Ninject DI](https://www.nuget.org/packages/Ninject/)

Предоставляет разграниченный доступ к телефонной книге:
* **Администратор** -- полный доступ к книге, администрирование пользователей
* **Пользователь** -- детальный просмотр записей, добавление новых записей, смена пароля
* **Анонимус** -- только просмотр записей

---

### SERVICES

* **PhonebookApi** -- сервисы поставщика контактов и идентификации / [Api.ApiContacts](https://www.nuget.org/packages/RozhkovSvyat.Project19.Services.Api.ApiContacts) + [Api.ApiIdentity](https://www.nuget.org/packages/RozhkovSvyat.Project19.Services.Api.ApiIdentity)
> :bulb: Фабрики возвращают объекты с внедренными зависимостями
* **Vms** -- фабрика моделей представлений и модель главного представления / [Tools.RecipeFactory](https://github.com/rozhkovsvyat/Tools.RecipeFactory) + [Ninject](https://www.nuget.org/packages/Ninject/)
* **Views** -- фабрика представлений / [Tools.RecipeFactory](https://github.com/rozhkovsvyat/Tools.RecipeFactory) + [Ninject](https://www.nuget.org/packages/Ninject/)
* **Validators** -- фабрика валидаторов ввода данных / [Tools.RecipeFactory](https://github.com/rozhkovsvyat/Tools.RecipeFactory) + [Ninject](https://www.nuget.org/packages/Ninject/)
