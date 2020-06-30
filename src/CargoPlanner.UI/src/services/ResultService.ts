import { httpGet, httpPost, httpPut, httpDelete } from './HttpService';
import * as result from '../models/Result';
import * as listItem from '../models/ListItem';

const url: string = 'https://localhost:5001/api/v1/results/';

export async function getResult(resultId: string): Promise<result.Result> {
  const response = await httpGet(url + resultId);
  const result: result.Result = await response.json();
  return result;
}

export async function getResults(userId: string): Promise<listItem.Result[]> {
  const response = await httpGet(url + userId + '/summary');
  const results: listItem.Result[] = await response.json();
  return results;
}
