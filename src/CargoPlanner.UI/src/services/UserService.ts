import React from 'react';
import User from '../models/User';
import { httpPost } from './HttpService';

const url: string = 'https://localhost:5001/api/v1/users/';

export async function register(user: User): Promise<string> {
  const response = await httpPost(url + '/register', user);
  const userId: string = await response.json();
  return userId;
}

export async function login(user: User): Promise<string> {
  const response = await httpPost(url + '/login', user);
  const userId: string = await response.json();
  return userId;
}

export function getToken() {
  return localStorage.getItem('token');
}

export function setUserId(userId: string) {
  localStorage.setItem('userId', userId);
}

export function getUserId() {
  // const userId = localStorage.getItem('userId');
  // if (userId !== null) {
  //   return userId.toString();
  // } else {
  //   return '';
  // }
  return '7f9fe276-c4d8-422a-8c42-00d46da6dcba';
}
