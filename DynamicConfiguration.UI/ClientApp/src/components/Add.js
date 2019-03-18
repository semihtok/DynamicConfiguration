import React, {Component} from 'react';
import {Button, Form, FormGroup, Input, Label, Row} from "reactstrap";

export class Add extends Component {
    static displayName = Add.name;

    constructor(props) {
        super(props);

        this.state = {
            applicationName: '',
            name: '',
            value: '',
            isActive: ''
        };
    }

    sendData = () => {

        const config = {
            Id: null,
            ApplicationName: this.state.applicationName,
            Name: this.state.name,
            Value: this.state.value,
            IsActive: this.state.isActive
        };

        var data = "ApplicationName="+config.ApplicationName+ "&Name="+config.Name+"&Value="+config.Value+"&IsActive=" + config.IsActive;

        var xhr = new XMLHttpRequest();
        xhr.withCredentials = true;

        xhr.addEventListener("readystatechange", function () {
            if (this.readyState === 4) {
                console.log(this.responseText);
            }
        });

        xhr.open("POST", "api/DynamicConfiguration");
        xhr.setRequestHeader("Content-Type", "application/x-www-form-urlencoded");
        xhr.setRequestHeader("cache-control", "no-cache");

        xhr.send(data);

    };

    updateAppInputValue = (e) => {
        this.setState({
            applicationName: e.target.value
        })
    };

    updateNameInputValue = (e) => {
        this.setState({
            name: e.target.value
        })
    };

    updateValueInputValue = (e) => {
        this.setState({
            value: e.target.value
        })
    };


    updateIsActiveInputValue = (e) => {
        this.setState({
            isActive: e.target.value
        });
    };

    render() {


        return (
            <Row>
                <div className={"col-md-12"}>

                    <div>
                        <Form>
                            <FormGroup>
                                <Label for="applicationName">Application Name</Label>
                                <Input type="text" name="applicationName" id="applicationName"
                                       onChange={this.updateAppInputValue}
                                       placeholder="Application Name"/>
                            </FormGroup>

                            <FormGroup>
                                <Label for="name">Name</Label>
                                <Input type="text" name="name" id="name" placeholder="Name"
                                       onChange={this.updateNameInputValue}/>
                            </FormGroup>

                            <FormGroup>
                                <Label for="value">Value</Label>
                                <Input type="text" name="value" id="value" placeholder="Value"
                                       onChange={this.updateValueInputValue}/>
                            </FormGroup>

                            <FormGroup>
                                <Label for="isActive">Is Active</Label>
                                <Input type="number" name="isActive" id="isActive" placeholder="0 or 1"
                                       onChange={this.updateIsActiveInputValue}/>
                            </FormGroup>

                            <Button className={"success"} onClick={this.sendData}>Save</Button>
                        </Form>

                    </div>

                </div>
            </Row>
        );
    }


}
