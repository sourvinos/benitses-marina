// ng build --output-path="release" --configuration=production-live

export const environment = {
    apiUrl: 'https://appsourvinos.com/api',
    url: 'https://appsourvinos.com',
    appName: 'Benitses Marina',
    clientUrl: 'https://appsourvinos.com',
    defaultLanguage: 'en-GB',
    featuresIconDirectory: 'assets/images/features/',
    cssUserSelect: 'auto',
    minWidth: 1280,
    login: {
        username: '',
        email: '',
        password: '',
        noRobot: false
    },
    production: true
}
