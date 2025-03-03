// ng build --output-path="release" --configuration=production-live

export const environment = {
    apiUrl: 'https://appbenitsesmarina.com/api',
    url: 'https://appbenitsesmarina.com',
    appName: 'Benitses Marina',
    clientUrl: 'https://appbenitsesmarina.com',
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
