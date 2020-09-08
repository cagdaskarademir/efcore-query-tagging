# Entity Framework Query Tagging Örneği

Bu repository https://medium.com/@cagdaskarademir/entityframework-query-tagging-d07b15e124e9 makalesini referans etmektedir.

## Projenin Docker-Compose İle Çalıştırılması

Proje için sql server veritabanına ihtiyaç vardır. docker-compose ile hızlı bir şekilde uygulamayı çalıştırabilirsiniz.

1) Projenizi veya docker-compose.yml dosyasını klonlayın

`git clone git@github.com:cagdaskarademir/efcore-query-tagging.git`

2) Docker Compose up ile container larınızı kurabilirsiniz.

`docker-compose up`


## Geliştirici Notları

Uygulama dotnet.core 3.1 console template'i kullanılarak yazılmıştır.

İlk aşamada uygulama ayağa kalkarken; veritabanının hazır olup olmadığını kontrol eder.

Eğer veritabanı hazır ise; code first aracılığıyla gerekli olan tablolar generate edilir.

Program.cs içerisinde yer alan RunSample1() fonksiyonu ile query tagging örneğini görebilirsiniz.
