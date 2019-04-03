import React, {Component} from 'react';
import {Button, Form, FormGroup, Input, Label, Modal, ModalBody, ModalFooter, ModalHeader, Row} from "reactstrap";

export class Edit extends Component {
    static displayName = Edit.name;

    constructor(props) {
        super(props);

        this.state = {
            query: '',
            configs: [],
            loading: true,
            searchString: [],
            key: '',
            modal: false
        };

        this.toggle = this.toggle.bind(this);
    }

    componentDidMount() {
        this.getData();
    }


    updateAppInputValue = (e) => {
        if (e.target.value !== undefined && e.target.value !== '') {

            this.state.configs.applicationName = e.target.value;
        }
    };

    updateNameInputValue = (e) => {
        if (e.target.value !== undefined && e.target.value !== '') {
            this.state.configs.name = e.target.value;
        }

    };

    updateValueInputValue = (e) => {
        if (e.target.value !== undefined && e.target.value !== '') {
            this.state.configs.value = e.target.value;
        }
    };


    updateIsActiveInputValue = (e) => {

        if (e.target.value !== undefined && e.target.value !== '') {
            this.state.configs.isActive = e.target.value;
        }
    };

    sendData = () => {

        const config = {
            Id: null,
            ApplicationName: this.state.configs.applicationName,
            Name: this.state.configs.name,
            Value: this.state.configs.value,
            IsActive: this.state.configs.isActive
        };

        const data = "ApplicationName=" + config.ApplicationName + "&Name=" + config.Name + "&Value=" + config.Value + "&IsActive=" + config.IsActive;

        const xhr = new XMLHttpRequest();
        xhr.withCredentials = true;

        xhr.addEventListener("readystatechange", function () {
            if (this.readyState === 4) {
                console.log(this.responseText);
            }
        });

        xhr.open("POST", "http://localhost:5003/api/DynamicConfiguration/Update");
        xhr.setRequestHeader("Content-Type", "application/x-www-form-urlencoded");
        xhr.setRequestHeader("cache-control", "no-cache");

        xhr.send(data);
        this.toggle();

    };

    getData = () => {

        fetch('http://localhost:5003/api/DynamicConfiguration/GetOne?key=' + this.props.match.params.key)
            .then(response => response.json())
            .then(data => {
                console.log(data);
                this.setState({
                    configs: data,
                    searchString: data
                })
            });
    };

    toggle() {
        this.setState(prevState => ({
            modal: !prevState.modal
        }));
    }

    render() {
        return (
            <Row>
                <div className={"col-md-12"}>
                    <Form>
                        <FormGroup>
                            <Label for="applicationName">Application Name</Label>
                            <Input type="text" name="applicationName" id="applicationName"
                                   onChange={this.updateAppInputValue}
                                   placeholder={this.state.configs.applicationName}/>
                        </FormGroup>

                        <FormGroup>
                            <Label for="name">Name</Label>
                            <Input type="text" name="name" id="name" placeholder={this.state.configs.name}
                                   onChange={this.updateNameInputValue}/>
                        </FormGroup>

                        <FormGroup>
                            <Label for="value">Value</Label>
                            <Input type="text" name="value" id="value" placeholder={this.state.configs.value}
                                   onChange={this.updateValueInputValue}/>
                        </FormGroup>

                        <FormGroup>
                            <Label for="isActive">Is Active</Label>
                            <Input type="number" name="isActive" id="isActive" placeholder={this.state.configs.isActive}
                                   onChange={this.updateIsActiveInputValue}/>
                        </FormGroup>

                        <Button className={"success"} onClick={() => this.sendData()}>Save</Button>
                    </Form>

                    <Modal isOpen={this.state.modal} toggle={this.toggle} className={this.props.className}
                           backdrop={this.state.backdrop}>
                        <ModalHeader toggle={this.toggle}>Update Result</ModalHeader>
                        <ModalBody>
                            Config updated successfully
                        </ModalBody>
                        <ModalFooter>
                            <Button color="primary" onClick={this.toggle}>Ok</Button>{' '}
                        </ModalFooter>
                    </Modal>

                </div>
            </Row>
        );
    }


}
