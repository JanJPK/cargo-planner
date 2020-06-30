import React from 'react';
import logo from '../../logo.svg';
import { Row, Col, Image, Jumbotron, Container, Button } from 'react-bootstrap';
import { LinkContainer } from 'react-router-bootstrap';

const Home = () => (
  <Jumbotron fluid>
    <Container>
      <Row className="justify-content-md-center">
        <Col md="auto">
          <h1>Cargo Planner</h1>
          <Image src={logo} alt="logo" fluid />
          <p>
            Cargo Planner provides 3D solutions for truck loading problem. Begin
            by creating a new instance, or loading an existing one.
          </p>
        </Col>
      </Row>
      <Row className="justify-content-md-center">
        <Col md="auto">
          {' '}
          <LinkContainer to="/cargo-edit/new">
            <Button variant="primary" size="lg">
              New cargo
            </Button>
          </LinkContainer>{' '}
          <LinkContainer to="/cargo-list">
            <Button variant="primary" size="lg">
              Load cargo
            </Button>
          </LinkContainer>{' '}
          <LinkContainer to="/result-list">
            <Button variant="primary" size="lg">
              View results
            </Button>
          </LinkContainer>
        </Col>
      </Row>
    </Container>
  </Jumbotron>
);

export default Home;
