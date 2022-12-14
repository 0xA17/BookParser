# BookParser

## О проекте

BookParser - это парсер данных о книгах, который собирает и формирует сведения в БД с ресурса [www.litmore.ru](https://litmore.ru/).

## Начало работы

### 1. Запуск проекта

Скачиваем исходный код проекта или выполняем команду:
```
git clone https://github.com/M-R-8/BookParser -recursive
```

### 2.	Установка зависимостей программного обеспечения

```
dotnet add package HandyControl --version 3.4.0
dotnet add package HtmlAgilityPack --version 1.11.46
dotnet add package linq2db --version 4.3.0
dotnet add package Microsoft.Toolkit.Mvvm --version 7.0.2
dotnet add package Microsoft.Windows.SDK.Contracts --version 10.0.18362.2005
dotnet add package Npgsql --version 7.0.0
dotnet add package System.ValueTuple --version 4.5.0
```

### 3.	Установка PostgreSQL

Переходим на [официальный сайт PostgreSQL](https://www.enterprisedb.com/downloads/postgres-postgresql-downloads) и скачиваем пакеты, которые подходят для вашей ОС.

### 4.	Настройка PostgreSQL

Запускаем pgAdmin и в "Databases" создаем таблицу с названием `LitmoreBooks`

<p align="center">
    <img align="center" src="https://telegra.ph/file/18c3f89f625a7716816c2.png">
</p>

или выполняем следующий запрос:

```SQL
CREATE DATABASE "LitmoreBooks"
    WITH
    OWNER = postgres
    ENCODING = 'UTF8'
    CONNECTION LIMIT = -1
    IS_TEMPLATE = False;
```

После создания базы необходимо добавить таблицу.
Открываем Query Tool:

<p align="center">
    <img align="center" src="https://telegra.ph/file/1d9cebf4a1e425209a6f9.png">
</p>

и выполняем следующий запрос:

```SQL
CREATE TABLE books (
    Title       TEXT,
    Author      TEXT,
    PublishYear TEXT,
    WriteYear   TEXT,
    Publisher   TEXT,
    Isbn        TEXT,
    Genres      TEXT,
    Series      TEXT,
    Description TEXT
);
```

### 4.	Настройка строки подключения

В файле [App.config](https://github.com/M-R-8/BookParser/blob/master/BookParser/App.config) необходимо проверить данные в `connectionStrings` на корректность, совпадают ли данные с вашими, которые устанавливали при загрузке PostgreSQL:

```XML
<?xml version="1.0" encoding="utf-8"?>

<configuration>
    <startup> 
        <supportedRuntime version="v4.0" sku=".NETFramework,Version=v4.8" />
    </startup>
	<connectionStrings>
		<add name="Books"
			 connectionString="Server=localhost;Port=5432;User Id=postgres;Password=123;Database=LitmoreBooks;"
			 providerName="PostgreSQL.15"/>
    </connectionStrings>
  <runtime>
    <assemblyBinding xmlns="urn:schemas-microsoft-com:asm.v1">
      <dependentAssembly>
        <assemblyIdentity name="System.ComponentModel.Annotations" publicKeyToken="b03f5f7f11d50a3a" culture="neutral" />
        <bindingRedirect oldVersion="0.0.0.0-4.2.1.0" newVersion="4.2.1.0" />
      </dependentAssembly>
      <dependentAssembly>
        <assemblyIdentity name="System.Buffers" publicKeyToken="cc7b13ffcd2ddd51" culture="neutral" />
        <bindingRedirect oldVersion="0.0.0.0-4.0.3.0" newVersion="4.0.3.0" />
      </dependentAssembly>
    </assemblyBinding>
  </runtime>
</configuration>
```

### 5. Компилируем и запускаем проект

После запуска наживаем на кнопку "Go parse"

<p align="center">
    <img align="center" src="https://telegra.ph/file/0c636fc59ebe579e51a54.png">
</p>

### 6. Просмотр результата

После успешного парсинга данных необходимо выполнить запрос для отображения добавленных данных о книгах:

```SQL
SELECT * FROM PUBLIC.books
```

<p align="center">
    <img align="center" src="https://telegra.ph/file/31f7d3a0f1687635a21ce.png">
</p>

## Участие
Некоторые из лучших способов внести свой вклад — попробовать что-то, зарегистрировать проблемы, присоединиться к обсуждениям дизайна и сделать запросы на вытягивание.

Есть много способов, которыми вы можете участвовать в этом проекте, например:

* Отправляйте сообщения об ошибках и запросы функций и помогайте нам проверять их по мере их регистрации.
* Просмотр изменений исходного кода
* Просмотрите документацию и отправьте запросы на вытягивание для всего, от опечаток до дополнительного и нового контента.