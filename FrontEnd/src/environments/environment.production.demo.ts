// ng build --output-path="release" --configuration=production-demo

export const environment = {
    apiUrl: 'https://appsourvinos.com/api',
    url: 'https://appsourvinos.com',
    appName: 'Benitses Marina',
    clientUrl: 'https://appsourvinos.com',
    defaultLanguage: 'el-GR',
    featuresIconDirectory: 'assets/images/features/',
    cssUserSelect: 'auto',
    minWidth: 1280,
    login: {
        username: '',
        email: '',
        password: '',
        noRobot: false
    },
    isDevelopment: false,
    isProduction: true
}
