# AlertaPrecoAtivo
Este projeto consiste de uma aplicação de console que verifica o preço de um determinado ativo, de escolha do usuário, da bolsa de valores americana, e alerta o usuário via email quando o preço fica abaixo de um valor ou acima de outro (valores que também são passados pelo usuário).

## Instalação e Execução
Caso deseje instalar a aplicação em sua máquina é necessário ter os seguintes recursos: <br/>
1. Visual Studio

2. Pacotes (caso precise instale-os via Gerenciador de Pacotes do NuGet):
   - RestSharp
   - Newtonsoft.Json
   - Microsoft.Extensions.Configuration
   - System.Threading.Tasks
   - System.Threading.Tasks.Extensions
   - System
   - System.Net
   - System.Net.Mail
   
3. Compile o programa
   - Para compilar clique com o botão direito do mouse sobre o nome do Projeto (AlertaPrecoAtivo) no Gerenciador de Soluções e clique em "Compilar"
   
4. Execute o programa pelo CMD ou pelo terminal do visual studio
   - Depois de compilar, o executavel provavelvemente estará na pasta AlertaPrecoAtivo\bin\Debug
   - O programa deve ser executado com o seguinte comando: <br/> 
   `AlertaPrecoAtivo.exe <COD_ACAO> <VALOR_VENDA> <VALOR_COMPRA>` <br/>
   ex: `AlertaPrecoAtivo.exe AAPL 180.43 150.34`

## Configurando o smtpconfig.json
O smtpconfig.json é um arquivo de configuração com o e-mail de destino dos alertas e as configurações de acesso ao servidor de SMTP que irá enviar o e-mail.<br/>
Ele possui o seguinte formato:
```
{
  "EmailServerConfig": {
    "EmailSource": "ENDEREÇO DE EMAIL DE ENVIO",
    "Name": "App Alerta de Preço",
    "Server": "SMTP SERVER",
    "Port": "PORTA SMTP SERVER",
    "User": "ENDEREÇO DE EMAIL DE ENVIO",
    "Password": "SENHA DO EMAIL DE ENVIO",
    "SSL": true,
    "Destination": "ENDEREÇO DE EMAIL DE DESTINO",
    "TimeUpdateMinutes": "TEMPO DE ATUALIZACAO DAS ACOES"
  }
}
```
Insira os dados de email e configuração do SMTP server em seus respectivos campos.<br/>

### Para cada serviço de email existe um SMTP SERVER específico. Segue uma lista dos principais:<br/>
- Gmail
  - Server: smtp.gmail.com;
  - Port: 465, caso esteja usando SSL ou 587 se for TLS.
  
- Hotmail
  - Server: smtp.live.com;
  - Port: 25 ou 465.
  
- Yahoo
  - Server: smtp.mail.yahoo.com;
  - Port: 465.

- Outlook.com
  - Server: SMTP.office365.com;
  - Port: 587.

## OBS:
Para uso do programa é necessário ter uma chave da API da [Finnhub](https://finnhub.io/dashboard). <br/> Quando você tiver sua chave você deve inseri-la no campo myApiKey em Program.cs.


## Exemplo de uso:
![image](https://user-images.githubusercontent.com/44558872/232075084-619af754-d0bb-4abd-816a-8a5f75620e44.png)
