import http from 'k6/http';
import { check, sleep } from 'k6';
import { Trend, Rate } from 'k6/metrics';

const createErrorRate = new Rate('NotificationErrors');
const CreateTrend = new Trend('Notification');

export const options = {
  thresholds: {
    'Notification': ['p(95)<800'],
  },
};

export default function () {
  const url = 'http://localhost:8082/Notification';
  const params = {
    headers: {
      'accept': '*/*',
      'limitType': 'RateLimited',
      'Content-Type': 'application/json',
    },
  };

  // Data for the POST request
  const endpointBody = JSON.stringify({
    "type": "Status",
    "recipient": {
        "name": "daniel",
        "emailAdress": "daniel.marketing@hotmail.com"
    },
    "message": {
        "title": "Important message",
        "body": "This message is Important"
  }
  });
  const requests = {
    'Notification': {
      method: 'POST',
      url: url,
      params: params,
      body: endpointBody,
    },
  };

  const responses = http.batch(requests);
  const createResp = responses['Notification'];

  check(createResp, {
    'status is 200': (r) => r.status === 200,
  }) || createErrorRate.add(1);

  CreateTrend.add(createResp.timings.duration);

  sleep(1);
}