# Project 19 : Phonebook WPF

<img align="right" width="100" height="100" src="https://github.com/rozhkovsvyat/Project19.WPF/assets/71471748/530dedd2-4fa9-4b5f-aaf3-61bce8b6b315">
<img align="right" width="100" height="100" src="https://github.com/rozhkovsvyat/Project19.WPF/assets/71471748/f40f2155-4c9a-4f8c-9754-4e10d46bd57c">

**#net7.0.10-windows**


WPF-клиент телефонной книги на базе [API](https://github.com/rozhkovsvyat/Project19.API)

Аналог [Web-клиента](https://github.com/rozhkovsvyat/Project19.Web), собранный на архитектуре MVVM

> :link: [Использует общие библиотеки](https://github.com/rozhkovsvyat/Project19.Libs)
>
> :link: [Использует иконки Bootstrap](https://www.nuget.org/packages/BootstrapIcons.Wpf)
> 
> :link: [Использует Ninject DI](https://www.nuget.org/packages/Ninject)
> 
> 💻 [Можно установить](https://hub.efcore.ru/PhonebookSetup.zip)

---

### SERVICES

* **PhonebookApi** -- поставщик контактов и идентификация / [Api.ApiContacts](https://www.nuget.org/packages/RozhkovSvyat.Project19.Services.Api.ApiContacts) + [Api.ApiIdentity](https://www.nuget.org/packages/RozhkovSvyat.Project19.Services.Api.ApiIdentity)

---

### FACTORIES

* **ValidatorFactory** -- валидаторы ввода данных форм / [Tools.RecipeFactory](https://github.com/rozhkovsvyat/Tools.RecipeFactory) + [Ninject](https://www.nuget.org/packages/Ninject)
* **VmFactory** -- модели представлений / [Tools.RecipeFactory](https://github.com/rozhkovsvyat/Tools.RecipeFactory) + [Ninject](https://www.nuget.org/packages/Ninject)
* **ViewFactory** -- представления / [Tools.RecipeFactory](https://github.com/rozhkovsvyat/Tools.RecipeFactory) + [Ninject](https://www.nuget.org/packages/Ninject)

---

### MVVM

* **LayoutVm** -- главный контекст данных, использует **ViewFactory** для привязки к мастер-окну, устанавливает содержимое используя **VmFactory**, хранит информацию о текущем пользователе и управляет [токеном](https://www.nuget.org/packages/Microsoft.AspNetCore.Authentication.JwtBearer) 
* **LayoutView** -- мастер-окно, для вывода содержимого использует фрейм с привязкой к контексту данных
* **Vm** -- контекст содержимого, использует **ViewFactory** для привязки к соответствующей странице, хранит ссылку на главный контекст и имеет доступ к токену
* **Frame** -- страница

---

💣 **404** notfound
💣 **500** exception
💣 **OOPS** apinotavailable
