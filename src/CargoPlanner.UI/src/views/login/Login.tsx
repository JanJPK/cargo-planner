import React, { useState } from 'react';
import { Form, Button, Card } from 'react-bootstrap';
import { login, register } from '../../services/UserService';
import User from '../../models/User';
import { RouteComponentProps } from 'react-router-dom';
import { setUserId } from '../../services/UserService';

function Login({ history }: RouteComponentProps) {
  const [user, setUser] = useState<User>({
    username: '',
    password: '',
  });
  return (
    <Card>
      <Card.Title>Login/Register</Card.Title>
      <Card.Body>
        <Form>
          <Form.Group>
            <Form.Label>Username</Form.Label>
            <Form.Control
              value={user.username}
              onChange={(e) => setUser({ ...user, username: e.target.value })}
            ></Form.Control>
          </Form.Group>
          <Form.Group>
            <Form.Label>Password</Form.Label>
            <Form.Control
              type="password"
              value={user.username}
              onChange={(e) => setUser({ ...user, username: e.target.value })}
            ></Form.Control>
          </Form.Group>
          {/* <Button onClick={handleRegister}>Register</Button>{' '}
          <Button onClick={handleLogin}>Login</Button>{' '}
          <Button variant="warning" onClick={skipLogin}>
            Skip login (debug)
          </Button> */}
        </Form>
      </Card.Body>
    </Card>
  );
}

export default Login;
