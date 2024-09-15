// ng build --output-path="release" --configuration=production-live

export const environment = {
    apiUrl: 'https://appbenitsesmarina.com/api',
    url: 'https://appbenitsesmarina.com',
    appName: 'Benitses Marina',
    clientUrl: 'https://appbenitsesmarina.com',
    defaultLanguage: 'en-GB',
    featuresIconDirectory: 'assets/images/features/',
    nationalitiesIconDirectory: 'assets/images/nationalities/',
    portStopOrdersDirectory: 'assets/images/port-stop-orders/',
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
