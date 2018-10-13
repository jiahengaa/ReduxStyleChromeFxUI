import axios from 'axios';

let BaseURL = '';


axios.defaults.headers.common['accessToken'] = 123;
axios.defaults.headers.common['projId'] = 213;

export {BaseURL,axios};