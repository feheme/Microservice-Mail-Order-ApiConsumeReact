import React, {Component} from "react";
import tutorialService from "../services/tutorial.service";

export default class AddTutorial extends Component{
    constructor(props){
        super(props);
        this.onChangeTitle = this.onChangeTitle.bind(this)
        this.onChangeDescription = this.onChangeDescription.bind(this)
        this.state = {
            id : null,
            title: "",
            description : "",
          
        }

    }

    tutorialSave(){
        console.log("tutorial saved click")
        const tobeSaved = {
            title : this.state.title,
            description : this.state.description,
            published : false
        }
        tutorialService.create(tobeSaved).
        then(newlyAddedTutorial => {
            console.log(newlyAddedTutorial)
            window.location.href="/tutorials"
        })
        .catch(error => {
            console.log("hata oluştu : " + error)
        })
    }

    onChangeTitle(e){
        this.setState({
            title : e.target.value
        })
    }
    onChangeDescription(e){
        this.setState({
            description : e.target.value
        })
    }
    
    render(){
        return(

            <div className="submit-form">
                <div className="form-group">
                    <label htmlFor="title">Title : </label>
                    <input 
                    type="text" 
                    className="form-control" 
                    id="title" 
                    required 
                    value={this.state.title}
                    onChange = {this.onChangeTitle}
                    ></input>
                </div>
            <br/>
                <div className="form-group">
                    <label htmlFor="description">Description : </label>
                    <input type="text" className="form-control" id="description" required value={this.state.description} onChange = {this.onChangeDescription} ></input>
                </div>
            <br/>
            <button className="btn btn-success" onClick={() => this.tutorialSave()} >Kaydet</button>

            </div>
        )
    }
 
}