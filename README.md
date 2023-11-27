# Project 19 : Phonebook WPF

<img align="right" width="100" height="100" src="https://github.com/rozhkovsvyat/Project19.WPF/assets/71471748/530dedd2-4fa9-4b5f-aaf3-61bce8b6b315">
<img align="right" width="100" height="100" src="https://github.com/rozhkovsvyat/Project19.WPF/assets/71471748/f40f2155-4c9a-4f8c-9754-4e10d46bd57c">

**#net7.0.10-windows**


WPF-клиент телефонной книги на базе [API](https://github.com/rozhkovsvyat/Project19.API)

Аналог [Web-клиента](https://github.com/rozhkovsvyat/Project19.Web), собранный на архитектуре MVVM

> :link: [Использует общие библиотеки](https://github.com/rozhkovsvyat/Project19.Libs)
>
> :link: [Использует иконки Bootstrap](https://www.nuget.org/packages/BootstrapIcons.Wpf)[/FontAwesome](https://www.nuget.org/packages/FontAwesome6.Svg)
> 
> :link: [Использует Ninject DI](https://www.nuget.org/packages/Ninject)

---

### SERVICES

* **PhonebookApi** -- поставщик контактов и идентификация / [Api.ApiContacts](https://www.nuget.org/packages/RozhkovSvyat.Project19.Services.Api.ApiContacts) + [Api.ApiIdentity](https://www.nuget.org/packages/RozhkovSvyat.Project19.Services.Api.ApiIdentity)

---

### FACTORIES

* **Validators** -- фабрика валидаторов ввода данных / [Tools.RecipeFactory](https://github.com/rozhkovsvyat/Tools.RecipeFactory) + [Ninject](https://www.nuget.org/packages/Ninject)
* **Vms** -- фабрика моделей представлений / [Tools.RecipeFactory](https://github.com/rozhkovsvyat/Tools.RecipeFactory) + [Ninject](https://www.nuget.org/packages/Ninject)
* **Views** -- фабрика представлений / [Tools.RecipeFactory](https://github.com/rozhkovsvyat/Tools.RecipeFactory) + [Ninject](https://www.nuget.org/packages/Ninject)

---

### LAYOUTVM

* Управляет содержимым фрейма, устанавливает вложенный контекст данных
* Содержит информацию о текущем пользователе, доступную вложенному контексту
* Управляет токеном, реализует переход на страницу авторизации

---

:bomb: **404** notfound
:bomb: **500** exception
:bomb: **OOPS** apinotavailable
