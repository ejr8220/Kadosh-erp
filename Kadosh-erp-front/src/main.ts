import 'zone.js';
import { bootstrapApplication } from '@angular/platform-browser';
import { appConfig } from './app/app.config';
import { App } from './app/app';
import { registerLicense } from '@syncfusion/ej2-base';
import { enableRipple } from '@syncfusion/ej2-base';

enableRipple(true);
registerLicense('Ngo9BigBOggjHTQxAR8/V1JFaF5cXGRCf1FpRmJGdld5fUVHYVZUTXxaS00DNHVRdkdmWH9ceHRVRWNYVUZwXURWYEg=');
bootstrapApplication(App, appConfig)
  .catch((err) => console.error(err));
