import { httpGet, httpPost, httpPut, httpDelete } from './HttpService';
import * as input from '../models/Input';
import * as listItem from '../models/ListItem';

const url: string = 'https://localhost:5001/api/v1/instances/';

export async function getInstance(instanceId: string): Promise<input.Instance> {
  const response = await httpGet(url + instanceId);
  const instance: input.Instance = await response.json();
  return instance;
}

export async function getInstances(
  userId: string,
): Promise<listItem.Instance[]> {
  const response = await httpGet(url + userId + '/summary');
  const instances: listItem.Instance[] = await response.json();
  return instances;
}

export async function postInstance(instance: input.Instance): Promise<string> {
  const json = JSON.stringify(instance);
  const response = await httpPost(url, json);
  const instanceId: string = await response.json();
  return instanceId;
}

export async function putInstance(
  instance: input.Instance,
  instanceId: string,
) {
  const json = JSON.stringify(instance);
  await httpPut(url + instanceId, json);
}

export async function deleteInstance(instanceId: string) {
  await httpDelete(url + instanceId);
}

export async function solveInstance(instanceId: string): Promise<string> {
  const response = await httpGet(url + instanceId + '/solve');
  const resultId: string = await response.json();
  return resultId;
}
